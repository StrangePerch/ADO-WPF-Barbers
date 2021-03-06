﻿using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace Barber
{
    public static class DataBaseConnector
    {
        private static SqlConnection _connection;
        private static SqlDataAdapter adapter;
        private static DataTable dataTable;
        private static SqlCommandBuilder builder;
        private static BarberShop barberShop;
        public static void Connect()
        {
            try
            {
                _connection = new SqlConnection
                {
                    ConnectionString = ConfigurationManager.ConnectionStrings["DataBase"].ConnectionString
                };
                _connection.Open();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Connection Error", MessageBoxButton.OK);
                throw;
            }
        }

        public static void Close()
        {
            try
            {
                _connection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Closing Error", MessageBoxButton.OK);
                throw;
            }
        }

        public static BarberShop GetBarberShop()
        {
            return barberShop ?? (barberShop = new BarberShop(System.Configuration.ConfigurationManager
                .ConnectionStrings["DataBase"].ConnectionString));
        }

        public static void DataGridConnect(DataGrid grid, string str_command)
        {
            SqlCommand command = new SqlCommand(str_command, _connection);
            adapter = new SqlDataAdapter(command);
            dataTable = new DataTable();
            adapter.Fill(dataTable);
            grid.ItemsSource = dataTable.DefaultView;
            builder = new SqlCommandBuilder(adapter);
        }

        public static DataTable GetDataTable(string str_command)
        {
            SqlCommand command = new SqlCommand(str_command, _connection);
            adapter = new SqlDataAdapter(command);
            dataTable = new DataTable();
            adapter.Fill(dataTable);
            return dataTable;
        }

        public static SqlDataReader GetReader(string str_command)
        {
            SqlCommand command = new SqlCommand(str_command, _connection);
            return command.ExecuteReader();
        }

        public static void Save()
        {
            try
            {
                adapter.Update(dataTable);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error while saving", MessageBoxButton.OK);
                throw;
            }
        }

        public static void NonQueryCommand(string str_command)
        {
            try
            {
                SqlCommand command = new SqlCommand(str_command, _connection);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error while saving", MessageBoxButton.OK);
                throw;
            }
        }

        public static int Count()
        {
            return dataTable.Rows.Count;
        }
    }
}