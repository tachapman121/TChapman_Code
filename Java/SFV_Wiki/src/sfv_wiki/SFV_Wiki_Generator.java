package sfv_wiki;

import java.io.File;
import java.util.Scanner;

/***
 * Main class responsible for generating wiki page
 * * HUGE THANKS to FAT team for putting together JSON data this uses
 * * http://github.com/D4RKONION/fatsfvframedatajson
 */
public class SFV_Wiki_Generator 
{
	private static String data;
	private final static String character = "Zeku (Young)"; // if does not exist in JSON, will fail
	private final static String discordLink = "QV77zFs";
	private final static String playersToWatch = "iDom";
	private final static String overheads = "f+MP, hcf+KK";
	private final static String lows = "crLK, crMK, crHK";
	private final static String HKD = "crHK (CH), Misogi" ;
	private final static String CCs = "f+HP, Senha Kassatsu, crHK"; // crush counters
	
	/**
	 * Main function to run and build character wiki
	 * @param args
	 */
	public static void main(String[] args) 
	{
		// read in JSON file (courtesy of FAT team, http://github.com/D4RKONION/fatsfvframedatajson)
		// SFV_ExcelParser.GetInfo("G", 40, true, false, false);
		
		try
		{
			File file = new File("sfv_data.json");
			Scanner s = new Scanner(file);
			s.useDelimiter(" ");
			while (s.hasNext())
				data += s.next() + " ";
			s.close();
			data = data.substring(4);
		}
		catch (Exception e){e.printStackTrace();}
		
		// build wiki; comment out sections don't want/need
		String wiki = "";
		
//		wiki += Header();
//		wiki += stats();
//		wiki += summary();
//		wiki += changes();
//		wiki += moveList();
//		wiki += discussions();
		wiki += frameData();
//		wiki += combos();
//		wiki += vSystem();
//		wiki += strategy(); 
//		wiki += category();
		
		// output final result
		System.out.println(wiki);
	}
	
	// Header for page, along 
	private static String Header()
	{
		// Header, portrait 
		String wiki = "{{SFVHeader}}\n\n";
		wiki += "== " + character + " ==\n";
		wiki += "{|\n" +
			"|{{SFVCharacterPortrait|" + character + "}}\n";
		return wiki;
	}
	
	// stats page
	private static String stats()
	{
		// Stats
		String wiki = "";
		try
		{
			SFV_Stat_Generator stat = new SFV_Stat_Generator();
			wiki += stat.generateStats(data, character, overheads, lows, HKD, CCs);
		}
		catch (Exception e){e.printStackTrace();}
		
		wiki += "|}\n";
		return wiki;
	}
	
	// summary page (bio, why play, who to watch)
	private static String summary()
	{
		String wiki = "";
		wiki += "{{SFVCharacterSummary|" + character + "|\n"
				+ "Bio\n" +
				"|\n" +
				"Why Play" +
				"|\n" + 
				"|" +
				playersToWatch +
				"}}\n";
		return wiki;
	}
	
	// season changes
	private static String changes()
	{
		// Current Season Laucnh - Current Mid-Season Update
		String wiki = "{{SFVCharacterChangeList |TBW|TBW}}\n\n";
		return wiki;
	}
	
	// move list images
	private static String moveList()
	{
		String wiki = "{{SFVCharacterMovelist|" + character + "}}\n\n";
		return wiki;
	}
	
	// discussions
	private static String discussions()
	{
		String wiki = "";
		// links to SRK forum pages, and Character Discord if available 
		wiki += "{{SFVCharacterDiscussions|" + character + "|" + character + "|" + discordLink + "}}\n\n";
		wiki += "== Video Guides == \n\n";
		wiki += "== Other Useful Guides == \n\n";
		
		return wiki;
	}
	
	// frame data
	private static String frameData()
	{
		String wiki = "== Frame Data ==\n";

		// generate frame data
		try
		{
			SFV_Frame_Generator frame = new SFV_Frame_Generator();
			String frameData = frame.Frame_Reader(character, data);
			wiki += frameData;
		}
		catch (Exception e){e.printStackTrace();}
		
		return wiki;
	}
	
	// combos
	private static String combos()
	{
		String wiki = "";
		// Combo Section
		wiki += "== Combos == \n" + 
				"=== BnB === \n" +
				"=== With Meter === \n" +
				"=== Corner === \n" +
				"=== Crush Counter === \n" +
				"=== V-Trigger Cancels === \n";
		
		return wiki;
	}
	
	private static String vSystem()
	{
		String wiki = "";
		// V-System
		wiki += "{{SFVCharacterVSystem\n" +
				"|\n" + // vr
				"|\n" + // vs1
				"|\n" + // vs2
				"|\n" + // vt1
				"|\n" + // vt2
				"}}\n";
		
		return wiki;
	}
	
	// strategy (playing as, fighting against)
	private static String strategy()
	{
		String wiki = "";
		
		// Strategy
		wiki += "== Strategy ==\n" +
				"=== Playing as " + character + " ===\n" +
				"=== Fighting Against " + character + " ===\n";
		
		return wiki;
	}
	
	// category for bottom of page
	private static String category()
	{
		String wiki = "";
		wiki +="[[Category: Street Fighter V]]";
		return wiki;
	}
}
