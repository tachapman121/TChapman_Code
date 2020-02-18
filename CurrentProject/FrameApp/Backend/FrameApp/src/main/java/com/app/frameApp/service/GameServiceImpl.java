package com.app.frameApp.service;

import java.util.List;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.mongodb.core.MongoTemplate;
import org.springframework.data.mongodb.core.query.Criteria;
import org.springframework.data.mongodb.core.query.Query;
import org.springframework.stereotype.Service;

import com.app.frameApp.model.Game;
import com.app.frameApp.repositories.GameRepository;

@Service
public class GameServiceImpl implements GameService {
	private final GameRepository gameRepo;
	@Autowired
	MongoTemplate mongoTemplate;
	
	public GameServiceImpl(GameRepository gameRepo) {
		this.gameRepo = gameRepo;
	}

	@Override
	public List<Game> getGames() {
		return gameRepo.findAll();
	}

	@Override
	public Game findGame(String name) {
		Query query = new Query(Criteria.where("gameId").is(name.toUpperCase()));
		Game game = mongoTemplate.findOne(query, Game.class);
		return game;
	}

	@Override
	public Game addGame(Game game) {
		return gameRepo.save(game);
	}

}
