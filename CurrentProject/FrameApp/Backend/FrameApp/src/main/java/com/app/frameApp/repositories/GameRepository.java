package com.app.frameApp.repositories;

import org.springframework.data.mongodb.repository.MongoRepository;

import com.app.frameApp.model.Game;

public interface GameRepository extends MongoRepository<Game, String> {
	
}
