/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package customerdb;

import java.io.Serializable;
import java.util.Collection;
import javax.persistence.Basic;
import javax.persistence.CascadeType;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.NamedQueries;
import javax.persistence.NamedQuery;
import javax.persistence.OneToMany;
import javax.persistence.Table;
import javax.xml.bind.annotation.XmlRootElement;
import javax.xml.bind.annotation.XmlTransient;
import org.codehaus.jackson.annotate.JsonIgnore;

@Entity
@Table(name = "MANUFACTURER")
@XmlRootElement
@NamedQueries({
    @NamedQuery(name = "Manufacturer.findAll", query = "SELECT m FROM Manufacturer m"),
    @NamedQuery(name = "Manufacturer.findByManufacturerId", query = "SELECT m FROM Manufacturer m WHERE m.manufacturerId = :manufacturerId"),
    @NamedQuery(name = "Manufacturer.findByName", query = "SELECT m FROM Manufacturer m WHERE m.name = :name"),
    @NamedQuery(name = "Manufacturer.findByAddressline1", query = "SELECT m FROM Manufacturer m WHERE m.addressline1 = :addressline1"),
    @NamedQuery(name = "Manufacturer.findByAddressline2", query = "SELECT m FROM Manufacturer m WHERE m.addressline2 = :addressline2"),
    @NamedQuery(name = "Manufacturer.findByCity", query = "SELECT m FROM Manufacturer m WHERE m.city = :city"),
    @NamedQuery(name = "Manufacturer.findByState", query = "SELECT m FROM Manufacturer m WHERE m.state = :state"),
    @NamedQuery(name = "Manufacturer.findByZip", query = "SELECT m FROM Manufacturer m WHERE m.zip = :zip"),
    @NamedQuery(name = "Manufacturer.findByPhone", query = "SELECT m FROM Manufacturer m WHERE m.phone = :phone"),
    @NamedQuery(name = "Manufacturer.findByFax", query = "SELECT m FROM Manufacturer m WHERE m.fax = :fax"),
    @NamedQuery(name = "Manufacturer.findByEmail", query = "SELECT m FROM Manufacturer m WHERE m.email = :email"),
    @NamedQuery(name = "Manufacturer.findByRep", query = "SELECT m FROM Manufacturer m WHERE m.rep = :rep")})
public class Manufacturer implements Serializable {
    private static final long serialVersionUID = 1L;
    @Id
    @Basic(optional = false)
    @Column(name = "MANUFACTURER_ID")
    private Integer manufacturerId;
    @Column(name = "NAME")
    private String name;
    @Column(name = "ADDRESSLINE1")
    private String addressline1;
    @Column(name = "ADDRESSLINE2")
    private String addressline2;
    @Column(name = "CITY")
    private String city;
    @Column(name = "STATE")
    private String state;
    @Column(name = "ZIP")
    private String zip;
    // @Pattern(regexp="^\\(?(\\d{3})\\)?[- ]?(\\d{3})[- ]?(\\d{4})$", message="Invalid phone/fax format, should be as xxx-xxx-xxxx")//if the field contains phone or fax number consider using this annotation to enforce field validation
    @Column(name = "PHONE")
    private String phone;
    // @Pattern(regexp="^\\(?(\\d{3})\\)?[- ]?(\\d{3})[- ]?(\\d{4})$", message="Invalid phone/fax format, should be as xxx-xxx-xxxx")//if the field contains phone or fax number consider using this annotation to enforce field validation

    @Column(name = "FAX")
    private String fax;
    // @Pattern(regexp="[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", message="Invalid email")//if the field contains email address consider using this annotation to enforce field validation

    @Column(name = "EMAIL")
    private String email;

    @Column(name = "REP")
    private String rep;
    @OneToMany(cascade = CascadeType.ALL, mappedBy = "manufacturerId")
    private Collection<Product> productCollection;

    public Manufacturer() {
    }

    public Manufacturer(Integer manufacturerId) {
        this.manufacturerId = manufacturerId;
    }

    public Integer getManufacturerId() {
        return manufacturerId;
    }

    public void setManufacturerId(Integer manufacturerId) {
        this.manufacturerId = manufacturerId;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getAddressline1() {
        return addressline1;
    }

    public void setAddressline1(String addressline1) {
        this.addressline1 = addressline1;
    }

    public String getAddressline2() {
        return addressline2;
    }

    public void setAddressline2(String addressline2) {
        this.addressline2 = addressline2;
    }

    public String getCity() {
        return city;
    }

    public void setCity(String city) {
        this.city = city;
    }

    public String getState() {
        return state;
    }

    public void setState(String state) {
        this.state = state;
    }

    public String getZip() {
        return zip;
    }

    public void setZip(String zip) {
        this.zip = zip;
    }

    public String getPhone() {
        return phone;
    }

    public void setPhone(String phone) {
        this.phone = phone;
    }

    public String getFax() {
        return fax;
    }

    public void setFax(String fax) {
        this.fax = fax;
    }

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public String getRep() {
        return rep;
    }

    public void setRep(String rep) {
        this.rep = rep;
    }

    @XmlTransient
    @JsonIgnore
    public Collection<Product> getProductCollection() {
        return productCollection;
    }

    public void setProductCollection(Collection<Product> productCollection) {
        this.productCollection = productCollection;
    }

    @Override
    public int hashCode() {
        int hash = 0;
        hash += (manufacturerId != null ? manufacturerId.hashCode() : 0);
        return hash;
    }

    @Override
    public boolean equals(Object object) {
        // TODO: Warning - this method won't work in the case the id fields are not set
        if (!(object instanceof Manufacturer)) {
            return false;
        }
        Manufacturer other = (Manufacturer) object;
        if ((this.manufacturerId == null && other.manufacturerId != null) || (this.manufacturerId != null && !this.manufacturerId.equals(other.manufacturerId))) {
            return false;
        }
        return true;
    }

    @Override
    public String toString() {
        return "customerdb.Manufacturer[ manufacturerId=" + manufacturerId + " ]";
    }
    
}
