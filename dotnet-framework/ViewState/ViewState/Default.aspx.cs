// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace ViewState
{
    /// <summary>
    /// Code-behind class for Default.aspx
    /// </summary>
    public partial class _Default : Page
    {
        // DataSet instance to hold data
        private DataSet _dataSet;

        /// <summary>
        /// Method to be executed when the page is loaded
        /// </summary>
        /// <param name="sender">The source of the event</param>
        /// <param name="e">The event data</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Loading customers data on initial page load
            if (IsPostBack == false)
            {
                // Loading data into the DataSet
                LoadData();

                // Binding the Customer table to the GridView
                grdCustomers.DataSource = _dataSet.Tables["Customer"];

                // Binding data to the GridView
                grdCustomers.DataBind();
            }
        }

        /// <summary>
        /// Method to handle row commands in the grdCustomers GridView
        /// </summary>
        /// <param name="sender">The source of the event</param>
        /// <param name="e">The event data</param>
        protected void grdCustomers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // Handling the ShowOrders command
            if (e.CommandName.Equals("ShowOrders"))
            {
                // Displaying the selected CustomerID
                lblCustomerId.Text = e.CommandArgument.ToString();

                // Loading data into the DataSet
                LoadData();

                // Creating a new DataTable to hold the filtered orders
                DataTable preOrdersTable = _dataSet.Tables["Order"];

                // Creating a new DataTable to hold the filtered orders
                DataTable ordersTable = new DataTable("Order");

                // Adding columns to the new DataTable
                foreach (DataColumn tempCol in preOrdersTable.Columns)
                {
                    ordersTable.Columns.Add(tempCol.ColumnName);
                }

                // Selecting rows that match the CustomerID
                DataRow[] dRows = _dataSet.Tables["Order"].Select("CustomerID = '" + e.CommandArgument.ToString() + "'");

                // Importing the selected rows into the new DataTable
                foreach (DataRow tempRow in dRows)
                {
                    ordersTable.ImportRow(tempRow);
                }

                // Binding the filtered orders to the grdOrders GridView
                grdOrders.DataSource = ordersTable;
                grdOrders.DataBind();

                // Making the relevant controls visible
                lblCustomerIdInfo.Visible = true;
                lblCustomerId.Visible = true;
                grdOrders.Visible = true;
            }
        }

        /// <summary>
        /// Method to handle row commands in the grdOrders GridView
        /// </summary>
        /// <param name="sender">The source of the event</param>
        /// <param name="e">The event data</param>
        protected void grdOders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // Handling the OrderDetails command
            if (e.CommandName.Equals("OrderDetails"))
            {
                // Displaying the selected OrderID
                lblOrderId.Text = e.CommandArgument.ToString();

                // Retrieving order details for the selected OrderID
                DataTable dt = GetOrderDetailsByOrderId(Int32.Parse(e.CommandArgument.ToString()));

                // Binding the order details to the grdOrderDetails GridView
                grdOrderDetails.DataSource = dt;
                grdOrderDetails.DataBind();

                // Making the relevant controls visible
                lblOrderIdInfo.Visible = true;
                lblOrderId.Visible = true;
                grdOrderDetails.Visible = true;
            }
        }

        /// <summary>
        /// Method to get order details by OrderID
        /// </summary>
        /// <param name="orderId">The ID of the order</param>
        private DataTable GetOrderDetailsByOrderId(int orderId)
        {
            // Loading the XML document
            XmlDocument doc = new XmlDocument();

            // Loading the northwind.xml file
            doc.Load(Server.MapPath("App_Data/northwind.xml"));

            // Selecting the OrderDetail nodes for the specified OrderID
            XmlNodeList nodes = doc.DocumentElement.SelectNodes("//Order[@OrderID=" + orderId + "]")[0].SelectNodes("OrderDetails")[0].SelectNodes("OrderDetail");

            // Converting the XmlNodeList to a DataTable
            DataTable dt = XmlNodeListToDataTable(nodes, new string[] { "ProductID", "UnitPrice", "Quantity", "Discount" });
            return dt;
        }

        /// <summary>
        /// Method to convert an XmlNodeList to a DataTable
        /// </summary>
        /// <param name="xmlNodeList">The XmlNodeList to convert</param>
        /// <param name="Columns">The columns to include in the DataTable</param>
        /// <returns>A DataTable representation of the XmlNodeList</returns>
        public DataTable XmlNodeListToDataTable(XmlNodeList xmlNodeList, string[] Columns)
        {
            // Creating the DataTable.
            using (DataTable dataTable = new DataTable("DataTable"))
            {
                // Adding data Table columns based on the columns parameter
                foreach (string column in Columns)
                {
                    dataTable.Columns.Add(column);
                }

                // Adding rows with values.
                DataRow dataRow;
                foreach (XmlNode node in xmlNodeList)
                {
                    dataRow = dataTable.NewRow();

                    // Filling the row with values from the XmlNode
                    foreach (string column in Columns)
                    {
                        dataRow[column] = node.SelectSingleNode(column).InnerText;
                    }
                    dataTable.Rows.Add(dataRow);
                }
                return dataTable;
            }
        }

        /// <summary>
        /// Method to handle page index changing in the grdCustomers GridView
        /// </summary>
        /// <param name="sender">The source of the event</param>
        /// <param name="e">The event data</param>
        protected void grdCustomers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Loading data into the DataSet
            LoadData();

            // Binding the Customer table to the GridView
            grdCustomers.DataSource = _dataSet.Tables["Customer"];
            grdCustomers.DataBind();

            // Setting the new page index
            grdCustomers.PageIndex = e.NewPageIndex;
            grdCustomers.DataBind();
        }

        /// <summary>
        /// Method to handle page index changing in the grdOrders GridView
        /// </summary>
        /// <param name="sender">The source of the event</param>
        /// <param name="e">The event data</param>
        protected void grdOders_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            // Loading data into the DataSet
            LoadData();

            // Binding the Order table to the GridView
            grdOrders.DataSource = _dataSet.Tables["Order"];
            grdOrders.DataBind();

            // Setting the new page index
            grdOrders.PageIndex = e.NewPageIndex;
            grdOrders.DataBind();
        }


        /// <summary>
        /// Method to handle page index changing in the grdOrderDetails GridView
        /// </summary>
        /// <param name="sender">The source of the event</param>
        /// <param name="e">The event data</param>
        protected void grdOrderDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            // Loading data into the DataSet
            LoadData();

            // Binding the OrderDetail table to the GridView
            grdOrderDetails.DataSource = _dataSet.Tables["OrderDetail"];
            grdOrderDetails.DataBind();

            // Setting the new page index
            grdOrderDetails.PageIndex = e.NewPageIndex;
            grdOrderDetails.DataBind();
        }

        /// <summary>
        /// Method to load data into the DataSet
        /// </summary>
        private void LoadData()
        {
            if (_dataSet == null)
            {
                _dataSet = new DataSet();
                _dataSet.ReadXml(Server.MapPath("App_Data/northwind.xml"));
            }
        }

    }
}
