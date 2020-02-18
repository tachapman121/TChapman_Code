package com.example.demo;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.CommandLineRunner;

import com.example.demo.Repositories.CustomerRepository;
import com.example.demo.model.Customer;

// CommandLineRunner: Run on startup
public class BootStrap implements CommandLineRunner {
	
	private final CustomerRepository customerRepo;
	
	@Autowired
	public BootStrap(CustomerRepository customerRepo) {
		this.customerRepo = customerRepo;
	}

	@Override
	public void run(String... args) throws Exception {
		Customer c1 = new Customer();
		c1.setFirstName("A");
		customerRepo.save(c1);
	}
}
