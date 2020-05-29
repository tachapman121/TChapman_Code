package sfv_wiki;

import java.io.BufferedReader;
import java.io.File;
import java.io.IOException;
import java.io.InputStreamReader;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.Scanner;
import java.util.Set;

import org.json.simple.*;
import org.json.simple.parser.*;

public class SFV_Frame_Generator 
{
	private BufferedReader b;
	public SFV_Frame_Generator()
	{}
	
	/**
	 * Generates frame data by reading in JSON file and parsing</br>
	 * <b>NOTE</b>: There are some fields in the wiki not in data such as Chip and CH that need to 
	 * be filled in manaully.
	 * @param name Character name to get
	 * @throws Exception
	 * @return Properly formatted Frame Data section
	 */
	public String Frame_Reader(String name, String data) throws Exception
	{
		// setup
		String normal_moves = "";
		String throw_moves = "";
		String super_moves = "";
		String special_moves = "";
		String vt_moves = "";
		String vs_moves = "";
		String alpha_moves = "";
		String command_moves = "";
		String vt1_moves = "";
		String vt2_moves = "";
		
		// get moves for given character
		JSONParser s = new JSONParser();
		Object o = s.parse(data);
		JSONObject roster = (JSONObject) o;
		JSONObject character = (JSONObject) roster.get(name);
		JSONObject moves = (JSONObject) character.get("moves");
		
		/* Parse moves list move by move to get data for each one*/
		// get normal 
		JSONObject normals = (JSONObject) moves.get("normal"); // special, super, vskill, throw, command-grab, normal
		for (Object o2 : normals.values())
		{
			JSONObject move = (JSONObject) o2;
			String result = createMove(move);
			
			// determine which type to add to
			String type = (String) move.get("moveType");
			
			// regular moves don't have common command
			if (type == null)
				continue;
			else if (type.equals("normal"))
			{
				if (move.get("cmnCmd") == null)
					normal_moves += result;
				else
					command_moves += result;
			}
			else if (type.equals("special") || type.equals("movement-special") || type.equals("command-grab"))
				special_moves += result;
			else if (type.equals("vtrigger"))
				vt_moves += result;
			else if (type.equals("vskill"))
				vs_moves += result;
			else if (type.equals("alpha"))
				alpha_moves += result;
			else if (type.equals("throw"))
				throw_moves += result;
			else if (type.equals("super"))
				super_moves += result;
			else if (type.equals("taunt"))
				special_moves += result;
			else
			{
				System.out.println("Could not parse special: " + type);
				continue;
			}
		}
		
		// VT1 moves
		JSONObject VT1 = (JSONObject) moves.get("vtOne");
		for (Object o3 : VT1.values())
		{
			JSONObject move = (JSONObject) o3;
			vt1_moves += createMove(move);
		}
		
		// VT2 moves
		JSONObject VT2 = (JSONObject) moves.get("vtTwo");
		for (Object o3 : VT2.values())
		{
			JSONObject move = (JSONObject) o3;
			vt2_moves += createMove(move);
		}
		
		// Constructs frame data in wiki format
		try { b.close(); } catch (IOException e) {}
		return constructFrameData(normal_moves, throw_moves, vs_moves, alpha_moves, special_moves, 
				super_moves, vt_moves, command_moves, vt1_moves, vt2_moves);
	}
	
	/***
	 * if null returns -, else gets property
	 * @param o
	 * @param property
	 * @return Property in JSON form if exists, "-" otherwise
	 */
	private static String getProperty(JSONObject o, String property)
	{
		Object o2 = o.get(property);
		if (o2 == null)
			return "-";
		else
			return (String) o2.toString();
	}
	
	private static String getNumber(String number, String type, String crush)
	{
		try
		{
			Integer num = Integer.parseInt(number);
			if (type == "counterhit")
				if (crush != "-")
					return crush;
				else
					num += 2;
			else if (type == "chip")
				num = (int) (num * 0.165);
			else if (type == "ch_damage")
				num = (int) (num * 1.2);
			else if (type == "ch_stun")
				num = (int) (num * 1.2);
			
			return num.toString();
		}
		catch (Exception e)
		{
			if (type == "counterHit")
				return "FILL_ME_IN_COUNTER_HIT";
			else if (type == "chip")
				return "CHIP";
			else if (type == "ch_damage")
				return "CH_DAMAGE";
			else if (type == "ch_stun")
				return "CH_STUN";
			else
				return "?";
		}
	}
	
	/**
	 * Gets information from JSON file into string formatted row for wiki
	 * @param move Move in JSON format
	 * @return String containing move in wiki format
	 * @throws ParseException 
	 */
	private String createMove(JSONObject move) throws ParseException
	{
		String moveName = getProperty(move, "moveName");
		String command = getProperty(move, "plnCmd");
		String damage = getProperty(move, "damage");
		String stun = getProperty(move, "stun");
		String hitLevel = getProperty(move, "attackLevel");
		String cancel = getProperty(move, "cancelsTo");
		String startup = getProperty(move, "startup");
		String active = getProperty(move, "active");
		String recovery = getProperty(move, "recovery");
		String onBlock = getProperty(move, "onBlock");
		String onHit = getProperty(move, "onHit");
		String counterHit;
		String crushAdv = getProperty(move, "crushAdv");
		
		/* Handle counter hits*/
		// if move crushes, has different property than standard OH+2
		if (crushAdv != "-")
		{
			JSONParser parse = new JSONParser();
			Object json = parse.parse(crushAdv);
			JSONObject obj = (JSONObject) json;
			counterHit = getProperty(obj, "ccAdv");
		}
		
		// OH advantage only applies if not a KD
		else if (onHit.equals("KD"))
			counterHit = onHit;
		else
			counterHit = getNumber(onHit, "counterhit", crushAdv);
		
		
		String no_rise = getProperty(move, "kd");
		String quick_rise = getProperty(move, "kdr");
		String back_rise = getProperty(move, "kdrb");
		String chip;
		
		// grey life is about .165 for normals; lights cannot cause
		if (getProperty(move, "moveType").equals("normal"))
			if (moveName.contains("LP") || moveName.contains("LK"))
				chip = "-";
			else
				chip = getNumber(damage, "chip", crushAdv);
		else
			chip = "?";
		String ch_damage = getNumber(damage, "ch_damage", crushAdv);
		String ch_stun = getNumber(stun, "ch_stun", crushAdv);
		String result = row_input(moveName, command, damage, stun, hitLevel, cancel, startup, 
				active, recovery, onBlock, onHit, counterHit, no_rise, quick_rise, 
				back_rise, chip, ch_damage, ch_stun);
		
		return result;
	}
	
	/**
	 * Takes in input, and outputs a SFVFrameDataRow
	 * @param moveName
	 * @param cmd_input
	 * @param damage
	 * @param stun
	 * @param hitLevel
	 * @param cancel
	 * @param startup
	 * @param active
	 * @param recovery
	 * @param OB
	 * @param OH
	 * @param CH
	 * @param no_rise
	 * @param quick_rise
	 * @param back_rise
	 * @param chip
	 * @param CH_damage
	 * @param CH_Stun
	 * @return formatted SFVFrameDataRow
	 */
	private String row_input(String moveName, String cmd_input, String damage, String stun, String hitLevel
			, String cancel, String startup, String active, String recovery, String OB, String OH, 
			String CH, String no_rise, String quick_rise, String back_rise, String chip, String CH_damage,
			String CH_Stun)
	{
		// setup
		String result = "{{SFVFrameDataRow";
		ArrayList<Object> list = new ArrayList<Object>(26);
		b = new BufferedReader(new InputStreamReader(System.in));
		
		// data
		try
		{
			list.add(moveName);
			list.add(""); 
			list.add(cmd_input);
			list.add(damage);
			list.add(stun); 
			list.add(hitLevel);
			list.add(cancel);
			list.add(startup);
			list.add(active);
			list.add(recovery);
			list.add(list.get(7));
			list.add(list.get(8));
			list.add(list.get(9));
			list.add(""); // dunno what used for
			list.add(""); // dunno what used for
			list.add(""); // dunno what used for
			list.add(""); // dunno what used for
			list.add(OB);
			list.add(OH);
			list.add(CH);
			list.add(no_rise);
			list.add(quick_rise);
			list.add(back_rise);
			list.add(chip);
			list.add(CH_damage);
			list.add(CH_Stun);
		}
		catch (Exception e)
			{ e.printStackTrace(); }
		
		// print
		for (int i = 0; i < list.size(); i++)
			result += " | " + list.get(i);
		
		// output
		result += "|||||||||||||||}}"; // adds bunch at end for some reason??
	
		return result + "\n";
	}
	
	/**
	 * Constructs frame data section after receiving all sections
	 * @param normal_moves stand, crouch, jump attacks
	 * @param throw_moves LP+LK and b+LP+LK
	 * @param vs_moves VSkill
	 * @param vr_moves V-Reversal
	 * @param special_moves Specials
	 * @param super_moves CA
	 * @param vt_moves HP+HK (VT1) and (VT2)
	 * @param command_moves command normals (f+MP)
	 * @param vt1_moves Any moves in VT1 state
	 * @param vt2_moves Any moves in VT2 state
	 * @return properly formatted Frame Data section for SRK wiki
	 */
	private String constructFrameData(String normal_moves, String throw_moves, String vs_moves, 
			String vr_moves, String special_moves, String super_moves, String vt_moves, String command_moves, 
			String vt1_moves, String vt2_moves)
	{
		// normals - command normals - throws - V-System - specials - super - VT1 - VT2
		StringBuilder wiki = new StringBuilder();
		wiki.append("{{SFVFrameDataTableHeader}}\n");
		wiki.append("{{SFVFrameDataHeader}}\n");
		wiki.append(normal_moves);
		wiki.append("{{SFVFrameDataHeader}}\n");
		wiki.append(throw_moves);
		wiki.append("{{SFVFrameDataHeader}}\n");
		wiki.append(vt_moves);
		wiki.append(vs_moves);
		wiki.append(vr_moves);
		wiki.append("{{SFVFrameDataHeader}}\n");
		wiki.append(special_moves);
		wiki.append("{{SFVFrameDataHeader}}\n");
		wiki.append(super_moves);
		wiki.append("{{SFVFrameDataHeader}}\n");
		wiki.append("{{SFVFrameDataRow|In VT1|||||||||||||||||||||||||||||||||||||||}}\n");
		wiki.append(vt1_moves);
		wiki.append("{{SFVFrameDataHeader}}\n");
		wiki.append("{{SFVFrameDataRow|In VT2|||||||||||||||||||||||||||||||||||||||}}\n");
		wiki.append(vt2_moves);
		wiki.append("|-\n");
		wiki.append("|}");
		wiki.append("\n=====Notes:=====\n\n");
		wiki.append("* CH Damage and stun is calculated by multiplying base values by 1.2. " + 
				"For multi-hitting moves this usually only applies to first hit; if only second hit connects it will get the x1.2 benefits.\n" + 
				"* For Crush Counters: J = Juggle, C = Crumple, KD = Knockdown\n" + 
				"* Chip damage for normals refers to grey life inflicted\n" + "");
		
		// returns formatted data
		return wiki.toString();
	}
			
}
