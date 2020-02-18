package com.example.demo.services;

import java.util.List;

import com.example.demo.model.Customer;

public interface CustomerService {
	Customer findCustomerById(Long id);
	List<Customer> findAllCustomers();
	Customer saveCustomer(Customer customer);
}
