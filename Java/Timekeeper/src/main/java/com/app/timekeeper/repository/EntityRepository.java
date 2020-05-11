package com.app.timekeeper.repository;

import org.springframework.data.mongodb.repository.MongoRepository;

import com.app.timekeeper.model.TimeEntity;

public interface EntityRepository extends MongoRepository<TimeEntity, String> {
	
}
