package com.app.timekeeper.controllers;

import java.util.List;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.app.timekeeper.model.Time;
import com.app.timekeeper.model.TimeEntity;
import com.app.timekeeper.model.TimeTotaled;
import com.app.timekeeper.service.TimeEntityService;

@RestController
@RequestMapping("/api/v1")
public class TimeEntityController {
    public static final Logger logger = LoggerFactory.getLogger(TimeEntityController.class);

	@Autowired
	private TimeEntityService service;
	
	/**
	 * Returns times associated with entity name
	 * @param name Name of entity to get results from. Use _ instead of spaces "My_Game" instead of "My Game"
	 * @return {"name": "Test", "times": { {"date": 05-01-20, "time": 1}, {"date": 05-02-20, "time": 2}}
	 */
	@GetMapping("/getTime/{name}")
	public ResponseEntity<List<Time>> getTimeForEntity(@PathVariable String name) {
		try {
			List<Time> times = service.timeForEntity(name);
			ResponseEntity<List<Time>> timeEntity = new ResponseEntity<List<Time>>(times, HttpStatus.OK);
			return timeEntity;
		} catch (Exception e) {
			return new ResponseEntity<List<Time>>(HttpStatus.INTERNAL_SERVER_ERROR);
		}
	}
	
	/**
	 * Gets total time for given week Sunday - Date
	 * @return
	 */
	@GetMapping("/getTimeWeek")
	public ResponseEntity<List<TimeTotaled>> getTimeForWeek() {
		try {
			List<TimeTotaled> times = service.timeForWeek();
			ResponseEntity<List<TimeTotaled>> timeEntity = new ResponseEntity<List<TimeTotaled>>(times, HttpStatus.OK);
			return timeEntity;
		} catch (Exception e) {
			return new ResponseEntity<List<TimeTotaled>>(HttpStatus.INTERNAL_SERVER_ERROR);
		}
	}
	
	/**
	 * Creates a new time entity for given entity name. The entity must already exist in the database
	 * @param name Name of entity
	 * @param hours Number of hours to add
	 * @return
	 */
	@PostMapping(path="/time", consumes="application/json")
	public ResponseEntity<String> addTime(@RequestBody String name, float hours) {
		try {
			service.addTime(name, hours);
			return new ResponseEntity<String>(HttpStatus.ACCEPTED);
		} catch(Exception e) {
			return new ResponseEntity<String>(HttpStatus.INTERNAL_SERVER_ERROR);
		}
	}
	
	/**
	 * Creates a new time entity for given entity name. The entity must already exist in the database
	 * @param id ObjectId of entity
	 * @param hours Number of hours to add
	 * @return
	 */
	// Takes ObjectID
	@PostMapping(path="/time", consumes="application/json")
	public ResponseEntity<String> addTime(@RequestBody Long id, float hours) {
		try {
			service.addTime(id, hours);
			return new ResponseEntity<String>(HttpStatus.ACCEPTED);
		} catch(Exception e) {
			return new ResponseEntity<String>(HttpStatus.INTERNAL_SERVER_ERROR);
		}
	}
	
	/**
	 * Adds new entity to database
	 * @param entity TimeEntity to add containing name and type
	 * @return
	 */
	@PostMapping(path="/entity", consumes="application/json")
	public ResponseEntity<String> addEntity(@RequestBody TimeEntity entity) {
		if (entity.getName() == null || entity.getType() == null)
			return new ResponseEntity<String>("Name and Type required", HttpStatus.BAD_REQUEST);
		try {
			service.addEntity(entity);
			return new ResponseEntity<String>(HttpStatus.ACCEPTED);
		} catch(Exception e) {
			return new ResponseEntity<String>(HttpStatus.INTERNAL_SERVER_ERROR);
		}
	}
}
