package com.alachisoft.ncache.springbootsample;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.cache.annotation.EnableCaching;
import org.springframework.context.annotation.ImportResource;

@SpringBootApplication
@EnableCaching
//@ImportResource("classpath:beans.xml")
public class SpringBootSampleApplication {

	public static void main(String[] args) {
		SpringApplication.run(SpringBootSampleApplication.class, args);
	}
}
