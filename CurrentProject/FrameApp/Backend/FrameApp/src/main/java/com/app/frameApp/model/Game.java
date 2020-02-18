package com.app.frameApp.model;

import org.springframework.data.annotation.Id;
import org.springframework.data.mongodb.core.mapping.Document;

import com.fasterxml.jackson.annotation.JsonIgnore;

import lombok.Data;

@Data
@Document(collection="games")
public class Game {
	@Id
	@JsonIgnore
	private String _id;
	private String name;
	private String gameId;
	private String franchise;
	private String[] mainSchema;
	private String[] additionalSchema;
	private String[] stats;
	private String[] fighters;
	private String boxArt;
	
	public String serialize() {
		StringBuilder result = new StringBuilder();
		result.append(
				"{\"gameId\": \"" + gameId + "\"" 
				+ ", \"name\": \"" + name + "\""
				+ ", \"franchise\": \"" + franchise + "\"" 
				+ ", \"mainSchema\": [");
		
		result = array(result, mainSchema);
		
		result.append("], \"additionalSchema\": [");
		result = array(result, additionalSchema);
		
		result.append("], \"stats\": [");
		result = array(result, stats);
		
		result.append("], \"fighters\": [");
		result = array(result, fighters);
		
		result.append( "]}");
		return result.toString();
	}
	
	/**
	 * Small helper function to convert array to JSON
	 * @param builder
	 * @param array
	 * @return
	 */
	private StringBuilder array(StringBuilder builder, String[] array) {
		for (String schema : array)
			builder.append("\"" + schema + "\",");
		builder.deleteCharAt(builder.length()-1);
		
		return builder;
	}
	
	public String serializeGameInfo() {
		return "{"
				+ "\"game\": " + "\"" + this.name + "\""
				+ ",\"franchise\": " + "\"" + this.franchise + "\""
				+ ", \"boxArt\": " + "\"" + this.boxArt + "\""
				+ ", \"gameId: \": " + this.gameId + "\""
				+ "}";
	}
}