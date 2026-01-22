package com.alachisoft.ncache.samples;

import com.alachisoft.ncache.spring.NCacheCacheManager;
import com.alachisoft.ncache.spring.configuration.SpringConfigurationManager;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.ApplicationArguments;
import org.springframework.boot.CommandLineRunner;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.cache.CacheManager;
import org.springframework.cache.annotation.*;
import org.springframework.context.ConfigurableApplicationContext;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.data.repository.CrudRepository;
import org.springframework.lang.NonNull;
import org.springframework.stereotype.Component;
import org.springframework.stereotype.Repository;
import org.springframework.stereotype.Service;

import java.io.Serializable;
import java.nio.file.Path;
import java.util.Date;
import java.util.List;
import java.util.Properties;

// Use NCache integration with Spring to seamlessly cache objects. Next time, Spring fetches these
// objects from the cache and saves an expensive database trip. We're using an H2 in-memory database
// that goes away when the process ends.

@SpringBootApplication
@EnableCaching
public class SpringDataCaching {
	public static void main(String[] args) {

		System.out.println("Sample showing Spring caching with NCache to save expensive database trips.\n");

		// Running spring application
		ConfigurableApplicationContext context = SpringApplication.run(SpringDataCaching.class, args);

		// Get a Spring Service to save objects in the DB. This also caches them with 5min absolute (TTL) expiration.
		BookService service = context.getBean(BookService.class);
		service.save(new Book(18001, "The Second Machine Age", "Erik Brynjolfsson, Andrew McAfee", new Date(2014, 0, 20)));
		service.save(new Book(18002, "Harry Potter and the Prisoner of Azkaban", "J.K. Rowling", new Date(1999, 6, 8)));
		service.save(new Book(18003, "Cloud Computing | Methodology, Systems, and Applications", "Lizhe Wang, Rajiv Ranjan", new Date(2011, 9, 3)));
		service.save(new Book(18004, "The Alchemist", "Paulo Coelho", new Date(19993, 0, 1)));

		// Print all saved books details
		printBooks(service.listAll());

		System.out.println("\nFinding book by isbn# ...");

		// Use Spring Service to get a book from the DB. It actually comes from the cache
		Book foundBook = service.findBookByIsbn(18001);
		printBookDetails(foundBook);

		// Use Spring Service to save "updated" book to the DB. It updates the cache too
		System.out.println("\nUpdating the book author in DB...");
		foundBook.setAuthor("New Author");
		Book UpdatedBook = service.update(foundBook);

		System.out.println("\nUpdated book info...");
		printBookDetails(UpdatedBook);


		System.out.println("\n" + "Sample completed successfully.");
		context.close();
		System.exit(0);
	}

	// Helper method to print book details.
	private static void printBooks(List<Book> books) {
		System.out.println("Total Books: " + books.size());

		System.out.println("BookID, ISBN, Title, Author");
		for (Book book : books) {
			System.out.println("- " + book.getId() + ", " + book.getISBN() + ", " + book.getTitle() + ", " + book.getAuthor());
		}
	}

	// Helper method to print book details.
	private static void printBookDetails(Book book) {
		System.out.println("BookID, ISBN, Title, Author");
		System.out.println("- " + book.getId() + ", " + book.getISBN() + ", " + book.getTitle() + ", " + book.getAuthor());
	}
}

// Class to get connected with cache.
@Configuration
class CachingConfiguration {
	@Autowired
	private ApplicationArguments applicationArguments;
	@Bean
	public CacheManager cacheManager() {
		String[] args = applicationArguments.getSourceArgs();
		String resource = Path.of(System.getenv("NCHOME"), "config/ncache-spring.xml").toString();

		SpringConfigurationManager springConfigurationManager = new SpringConfigurationManager();
		springConfigurationManager.setConfigFile(resource);

		Properties properties = new Properties();
		properties.setProperty("ncacheid-instance", args[0]);

		NCacheCacheManager cacheManager = new NCacheCacheManager();
		cacheManager.setSpringConfigurationManager(springConfigurationManager);

		return cacheManager;
	}
}

//Helper class to perform CRUD operations.
@Service
class BookService {
	@Autowired
	private BookRepository repo;

	public List<Book> listAll() {
		return repo.findAll();
	}

	@CachePut(value = "books", key = "#book.getISBN()")
	public Book save(Book book) { return repo.save(book); }

	@CachePut(value = "books", key = "#book.getISBN()")
	public Book update(Book book) { return repo.save(book); }

	@Cacheable(value = "books", key = "#id")
	public Book get(int id) {
		return repo.findById(id);
	}

	@Cacheable(value = "books", key = "#isbn")
	public Book findBookByIsbn(long isbn) {
		return repo.findBookByIsbn(isbn);
	}

	@CacheEvict(value = "books", allEntries = true)
	public void delete(int id) {
		repo.deleteById(id);
	}
}

@Repository
interface BookRepository extends CrudRepository<Book, Integer> {
	Book findById(int id);
	@NonNull
	List<Book> findAll();
	void deleteById(int id);
	Book findBookByIsbn(long isbn);
}

// Book class to store book details.
@Entity
class Book implements Serializable {
	@Id
	@GeneratedValue(strategy = GenerationType.AUTO)
	private Integer id;
	private long isbn;
	private String title;
	private String author;
	private Date publishedDate;

	public Book() {
	}

	public Book(long isbn, String title,
				String author, Date publishedDate) {
		this.isbn = isbn;
		this.title = title;
		this.author = author;
		this.publishedDate = publishedDate;
	}

	public void setAuthor(String author) { this.author = author; }
	public long getISBN() { return isbn; }
	public void setId(Integer id) { this.id = id; }
	public void setISBN(long isbn) { this.isbn = isbn; }
	public Integer getId() { return id;	}
	public String getTitle() { return title; }
	public void setTitle(String title) { this.title = title; }
	public String getAuthor() { return author; }
	public Date getPublishedDate() { return publishedDate; }
	public void setPublishedDate(Date publishedDate) { this.publishedDate = publishedDate; }
}

//ncache-spring.xml:

//<application-config default-cache-name="books">
//<caches>
//<cache name="books" ncacheid-instance="democache" priority="normal" expiration-type="absolute" expiration-period="300"/>
//</caches>
//</application-config>