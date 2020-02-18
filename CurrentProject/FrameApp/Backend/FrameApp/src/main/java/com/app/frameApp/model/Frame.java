package com.app.frameApp.model;

import org.bson.types.ObjectId;
import org.springframework.data.annotation.Id;
import org.springframework.data.mongodb.core.mapping.Document;

import lombok.Data;
import lombok.Getter;
import lombok.Setter;

@Data
@Document(collection = "frames")
public class Frame {
	@Id
	private String _id;
	private String game;
	private String name;
	private State states;
	private String[] stats;
	private String[] otherSchema;
	private ObjectId user;
}
