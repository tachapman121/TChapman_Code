package com.app.frameApp.service;

import java.util.List;

import com.app.frameApp.model.Game;

public interface GameService {
	List<Game> getGames();
	Game findGame(String name);
	Game addGame(Game game);
}
