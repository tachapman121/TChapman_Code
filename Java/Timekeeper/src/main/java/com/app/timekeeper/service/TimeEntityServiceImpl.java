package com.app.timekeeper.service;

import java.time.DayOfWeek;
import java.time.LocalDate;
import java.time.temporal.TemporalAdjusters;
import java.util.Date;
import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.mongodb.core.MongoTemplate;
import org.springframework.data.mongodb.core.aggregation.Aggregation;
import org.springframework.data.mongodb.core.aggregation.AggregationResults;
import org.springframework.data.mongodb.core.query.Criteria;
import org.springframework.data.mongodb.core.query.Query;
import org.springframework.data.mongodb.core.query.Update;
import org.springframework.stereotype.Service;

import com.app.timekeeper.model.Time;
import com.app.timekeeper.model.TimeEntity;
import com.app.timekeeper.model.TimeTotaled;
import com.app.timekeeper.repository.EntityRepository;

@Service
public class TimeEntityServiceImpl implements TimeEntityService {
	private final EntityRepository repo;
	@Autowired
	MongoTemplate mongoTemplate;
	
	public TimeEntityServiceImpl(EntityRepository repo) {
		this.repo = repo;
	}

	/**
	 * Adds a new entity to database, using current date as the date added
	 */
	@Override
	public boolean addEntity(TimeEntity entity) {
		try {
			entity.setFirstDate(new Date());
			repo.save(entity);
			return true;
		} catch(Exception e) { // expand out based on error type
			return false;
		}
	}

	@Override
	public int addTime(String name, float hours) {
		try {
			// check that name already in DB
			Time time = new Time(hours);
			Query query = new Query();
			query.addCriteria(Criteria.where("name").is(name));
			return timeQuery(query, time);
		} catch(Exception e) {
			return -2;
		}
	}
	
	@Override
	public int addTime(Long id, float hours) {
		try {
			// check that name already in DB
			Time time = new Time(hours);
			Query query = new Query();
			query.addCriteria(Criteria.where("_id").is(id));
			return timeQuery(query, time);
		} catch(Exception e) {
			return -2;
		}
	}
	
	/**
	 * Helper method for querying database
	 * @param query Query to execute to search for time
	 * @param time Time object to add to entity
	 * @return -1 if entity does not exist in database, total time otherwise
	 */
	private int timeQuery(Query query, Time time) {
		TimeEntity entity = mongoTemplate.findOne(query, TimeEntity.class);
		if(entity == null) {
			return -1;
		}
		
		// update times and return
		entity.getTimes().add(time);
		Update update = new Update();
		update.set("times", entity.getTimes());
		mongoTemplate.updateFirst(query, update, TimeEntity.class);
		
		// return total time spent on entity after insertion
		return entity.getTotalTime();
	}

	@Override
	public List<TimeTotaled> timeForWeek() {
		// get closest previous Sunday
		LocalDate prevSunday = LocalDate.now().with(TemporalAdjusters.previousOrSame(DayOfWeek.SUNDAY));
		
		// unwind array, get records >= previous Sunday, and group for total
		Aggregation aggregate = Aggregation.newAggregation(
				Aggregation.unwind("times"), 
				Aggregation.match(Criteria.where("times.addDate").gte(prevSunday)), 
				Aggregation.group("name").sum("times.time").as("totalTime"));
		
		AggregationResults<TimeTotaled> results = mongoTemplate.aggregate(aggregate, TimeEntity.class, TimeTotaled.class);
		return results.getMappedResults();
	}

	@Override
	public List<Time> timeForEntity(String name) {
		name = name.replaceAll("_", " ");
		Query query = new Query(Criteria.where("name").regex(".*" + name + ".*")); // contains
		TimeEntity entity = mongoTemplate.findOne(query, TimeEntity.class);
		
		// if no entries found, otherwise return
		if (entity == null)
			return null;
		return entity.getTimes();
	}
	
	@Override
	public List<Time> timeForEntity(Long id) {
		Query query = new Query(Criteria.where("id").is(id)); // contains
		TimeEntity entity = mongoTemplate.findOne(query, TimeEntity.class);
		
		// if no entries found, otherwise return
		if (entity == null)
			return null;
		return entity.getTimes();
	}
}
