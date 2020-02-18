package sfv_wiki;

import org.json.simple.JSONObject;
import org.json.simple.parser.JSONParser;

/**
 * Reads in and generates stats for given character. Uses JSON Simple Library for parsing, can 
 * be downloaded here https://code.google.com/archive/p/json-simple/ 
 */
public class SFV_Stat_Generator 
{
	public SFV_Stat_Generator()
	{}

	/**
	 * Generates stats for a given character by parsing FAT JSON file
	 * 
	 * @param data JSON Data
	 * @param name Character name
	 * @param overheads
	 * @param lows
	 * @param HKD Hard Knock Downs
	 * @param CC Crush Counters
	 * @return
	 * @throws Exception
	 */
	public String generateStats(String data, String name, String overheads, 
			String lows, String HKD, String CC) throws Exception
	{
		JSONParser s = new JSONParser();
		Object o = s.parse(data);
		JSONObject roster = (JSONObject) o;
		JSONObject character = (JSONObject) roster.get(name);
		JSONObject stats = (JSONObject) character.get("stats");
		
		String stats_string = "{{SFVCharacterData|";
		stats_string += getProperty(stats, "health");
		stats_string += getProperty(stats, "stun");
		stats_string += getProperty(stats, "fWalk");
		stats_string += getProperty(stats, "bWalk");
		stats_string += getProperty(stats, "fDash");
		stats_string += getProperty(stats, "bDash");
		stats_string += getProperty(stats, "fDashDist");
		stats_string += getProperty(stats, "bDashDist");
		stats_string += getProperty(stats, "jumpApex"); // not in data, change?
		stats_string += getProperty(stats, "nJump");
		stats_string += getProperty(stats, "fJumpDist");
		stats_string += getProperty(stats, "bJumpDist");
		stats_string += getProperty(stats, "throwRange");
		stats_string += getProperty(stats, "throwHurt");
		stats_string += overheads + "|";
		stats_string += lows + "|";
		stats_string += HKD + "|";
		stats_string += CC + "|";
		stats_string += "}}\n\n";
		
		return stats_string;
	}
	
	/***
	 * if null returns -, else gets property
	 * @param o
	 * @param property
	 * @return
	 */
	private static String getProperty(JSONObject o, String property)
	{
		Object o2 = o.get(property);
		if (o2 == null)
			return "-|";
		else
			return (String) o2.toString() + "|";
	}
}
