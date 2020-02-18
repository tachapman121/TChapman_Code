package com.app.frameApp.controllers;

import java.util.List;

import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.app.frameApp.model.Game;
import com.app.frameApp.service.GameService;

@RestController
@RequestMapping(GameController.base_url)
public class GameController {
	private final GameService gameService;
	public static final String base_url = "/api/v1/game";
	
	public GameController(GameService gameService) {
		this.gameService = gameService;
	}
	
	@GetMapping("/{id}")
	public String getGame(@PathVariable String id) {
		return gameService.findGame(id).serialize();
	}
	
	@GetMapping
	public String getGames() {
		List<Game> games = gameService.getGames();
		StringBuilder builder = new StringBuilder();
		builder.append("{\"games\": [");
		for (Game game : games) {
			builder.append(game.serializeGameInfo());
			builder.append(",");
		}
		
		// remove last ,
		builder.deleteCharAt(builder.length()-1);
		builder.append("]}");
		return builder.toString();
	}
}
