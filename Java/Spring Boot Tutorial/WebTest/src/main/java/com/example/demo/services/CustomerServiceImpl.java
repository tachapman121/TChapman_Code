package com.example.demo.services;

import java.util.List;

import org.springframework.stereotype.Service;

import com.example.demo.Repositories.CustomerRepository;
import com.example.demo.model.Customer;

@Service
public class CustomerServiceImpl implements CustomerService {

	private final CustomerRepository customerRepo;
	
	public CustomerServiceImpl(CustomerRepository customerRepo) {
		this.customerRepo = customerRepo;
	}
	
	@Override
	public Customer findCustomerById(Long id) {
		return customerRepo.findById(id).get();
	}

	@Override
	public List<Customer> findAllCustomers() {
		return customerRepo.findAll();
	}
	
	public Customer saveCustomer(Customer customer) {
		return customerRepo.save(customer);
	}
}
