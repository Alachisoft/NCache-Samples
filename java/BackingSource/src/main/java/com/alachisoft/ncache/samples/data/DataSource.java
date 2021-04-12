package com.alachisoft.ncache.samples.data;

import java.sql.*;
import java.util.*;

public class DataSource {
    private Connection _conn;
    Statement _st;

    public void Init(String connString, String user, String pass) throws SQLException {
        establishConnection(connString, user, pass);
        _st = _conn.createStatement();
    }

    private void establishConnection(String connString, String pass, String user){
        try {
            _conn = DriverManager.getConnection(connString, user, pass);
        } catch (SQLException throwable) {
            throwable.printStackTrace();
        }
    }

    public void closeConnection() throws SQLException {
        _conn.close();
    }

    public Object loadCustomer(Object key) throws SQLException {
        String selectQuery = "Select * from CUSTOMERS WHERE CUSTOMERID = '"+ key +"'";
        ResultSet rs = _st.executeQuery(selectQuery);
        if(rs.next()){
            Customer customer = fetchCustomer(rs);
            rs.close();
            return customer;
        }
        return null;
    }

    /**
     * Loads a  "Customer" object from the datasource.
     * @param requestedCompanyName
     * @return
     */
    public long getCustomerCountByCompanyName(String requestedCompanyName) throws SQLException {
        long result = 0;

        String query = "Select COUNT(*) as total from CUSTOMERS WHERE COMPANYNAME = = '"+requestedCompanyName+"'";
        ResultSet rs = _st.executeQuery(query);

        if(rs.next()){
            result = rs.getLong("total");
        }
        rs.close();
        return result;
    }

    /**
     * Loads a  "Customer" object from the datasource.
     * @param key
     * @return
     * @throws SQLException
     */
    public List<Object> loadCustomersFromCountry(Object key) throws SQLException {
        List<Object> result = new ArrayList<>();

        String query = "Select * from CUSTOMERS WHERE COUNTRY = '"+key+"'";

        ResultSet rs = _st.executeQuery(query);

        while (rs.next()){
            Customer object = fetchCustomer(rs);
            result.add(object);
        }

        rs.close();

        return result;
    }

    /**
     * Loads a  "Customer" object from the datasource.
     * @param key
     * @return
     * @throws SQLException
     */
    public Queue<Object> loadCustomersByOrder(String key) throws SQLException {
        Queue<Object> result = new LinkedList<>();

        String query = "Select CUSTOMERID, COMPANYNAME, CONTACTNAME, CONTACTTITLE, ADDRESS,  " +
                "ADDRESS, CITY, REGION, POSTALCODE, COUNTRY, PHONE, FAX FROM CUSTOMERS ORDER BY COUNTRY ASC";

        ResultSet rs = _st.executeQuery(query);

        while (rs.next()){
            Customer object = fetchCustomer(rs);
            result.add(object);
        }

        rs.close();

        return result;
    }

    /**
     * Loads a "Customer" object from the datasource.
     * @param key
     * @return
     * @throws SQLException
     */
    public Map<String, Object> LoadCustomersByCity(String key) throws SQLException {
        Map<String, Object> result = new HashMap<>();

        String query = "Select CUSTOMERID, COMPANYNAME, CONTACTNAME, CONTACTTITLE, ADDRESS,  " +
                "ADDRESS, CITY, REGION, POSTALCODE, COUNTRY, PHONE, FAX FROM CUSTOMERS WHERE CITY = '"+key+"'";

        ResultSet rs = _st.executeQuery(query);

        while (rs.next()){
            Customer object = fetchCustomer(rs);
            result.put(object.getCustomerID(), object);
        }

        rs.close();

        return result;
    }

    /**
     * Loads a  "Customer" object from the datasource.
     * @param key
     * @return
     * @throws SQLException
     */
    public HashSet<Integer> LoadOrderIDsByCustomer(Object key) throws SQLException {
        HashSet<Integer> results = new HashSet<>();

        String query = "Select ORDERID FROM ORDERS WHERE CUSTOMERID = '"+key +"'";

        ResultSet rs = _st.executeQuery(query);

        while (rs.next()){
            results.add(rs.getInt(1));
        }
        rs.close();
        return results;
    }

    /**
     * Save Customer information to datasource
     * @param customer
     * @return
     */
    public boolean saveCustomer(Customer customer) throws SQLException {
        int rowChanged = 0;
        String sql = "insert into CUSTOMERS values('"
                + customer.getCustomerID() + "','"
                + customer.getCompanyName() + "','"
                + customer.getContactName() + "','"
                + customer.getContactTitle() + "','"
                + customer.getAddress() + "','"
                + customer.getCity() + "','"
                + customer.getRegion() + "','"
                + customer.getPostalCode() + "','"
                + customer.getCountry() + "','"
                + customer.getPhone() + "','"
                + customer.getFax() + "')";

        rowChanged = _st.executeUpdate(sql);
        return rowChanged > 0;
    }

    /**
     * Add <see cref="Customer"/> information to datasource.
     * @param customer
     * @return
     * @throws SQLException
     */
    public boolean addToSource(Customer customer) throws SQLException {
        int rowChanged = 0;
        String sql = "insert into CUSTOMERS values('"
                + customer.getCustomerID() + "','"
                + customer.getCompanyName() + "','"
                + customer.getContactName() + "','"
                + customer.getContactTitle() + "','"
                + customer.getAddress() + "','"
                + customer.getCity() + "','"
                + customer.getRegion() + "','"
                + customer.getPostalCode() + "','"
                + customer.getCountry() + "','"
                + customer.getPhone() + "','"
                + customer.getFax() + "')";

        rowChanged = _st.executeUpdate(sql);
        return rowChanged > 0;
    }

    /**
     *  Delete <see cref="Customer"/> information from datasource.
     * @param customer
     * @return
     * @throws SQLException
     */
    public boolean removeFromSource(Customer customer) throws SQLException {
        int rowChanged = 0;
        String sql = "delete from CUSTOMERS where CUSTOMERID = '"+customer.getCustomerID()+"'";
        rowChanged = _st.executeUpdate(sql);
        return rowChanged > 0;
    }


    private Customer fetchCustomer(ResultSet rs) throws SQLException {
        Customer customer = new Customer();
        customer.setCustomerID(rs.getString(1));
        customer.setCompanyName(rs.getString(2));
        customer.setContactName(rs.getString(3));
        customer.setContactTitle(rs.getString(4));
        customer.setAddress(rs.getString(5));
        customer.setCity(rs.getString(6));
        customer.setRegion(rs.getString(7));
        customer.setPostalCode(rs.getString(8));
        customer.setCountry(rs.getString(9));
        customer.setPhone(rs.getString(10));
        customer.setFax(rs.getString(11));
        return customer;
    }
}
