package sfv_wiki;

import java.io.File;
import com.incesoft.tools.excel.xlsx.*;

// G = 44
public class SFV_ExcelParser 
{
	/**
	 * Pulls in data from spreadsheet and converts to JSON
	 * @param charName Name of character
	 * @param sheetNumber sheet number to pull data from
	 * @param getMoves boolean to get moves
	 * @param getVT1 
	 * @param getVT2
	 * @return
	 */
	public static String GetInfo(String charName, int sheetNumber, boolean getMoves, boolean getVT1, boolean getVT2)
	{
		String json = '"' + charName + '"' + ":{" + '"' + "moves" + '"' + ":";
		File file = new File("SF5 Frame Data - FAT .xlsx");
		SimpleXLSXWorkbook wb = new SimpleXLSXWorkbook(file);
		Sheet moveSheet;
		Sheet vt1Sheet;
		Sheet vt2Sheet;
		
		if (getMoves)
		{
			moveSheet = wb.getSheet(sheetNumber);
			json += getMoves(moveSheet);
		}
		
		if (getVT1)
		{
			if (getMoves)
				vt1Sheet = wb.getSheet(sheetNumber+1);
			else
				vt1Sheet = wb.getSheet(sheetNumber);
			json += getVT1(vt1Sheet);
		}
		
		if (getVT2)
		{
			if (getMoves && getVT1)
				vt2Sheet = wb.getSheet(sheetNumber+2);
			else if (getVT1)
				vt2Sheet = wb.getSheet(sheetNumber+1);
			else
				vt2Sheet = wb.getSheet(sheetNumber);
			
			json += getVT2(vt2Sheet);
		}
		
		json += "}";
		return json;
	}
	
	private static String getMoves(Sheet sheet)
	{
		String json = "{" + '"' + "normal" + '"' + ":{";
		for (int row = 1; row < sheet.getRowCount(); row++)
		{
			json += '"' + sheet.getCellValue(row, 0) + '"' + ":{";
			for (int col = 1; col < 40; col++)
			{
				json += '"' + sheet.getCellValue(0, col) + '"' + ":";
				json += checkIfNumber(col, sheet.getCellValue(row, col));
			}
			json += "}";
			if (row > 1 && row < sheet.getRowCount())
				json += ",";
		}
		return json;
	}
	
	private static String getVT1(Sheet sheet)
	{
		String json = "{" + '"' + "vtOne" + '"' + ":{";
		return json;
	}
	
	private static String getVT2(Sheet sheet)
	{
		String json = "{" + '"' + "vtTwo" + '"' + ":{";
		return json;
	}
	
	private static String getStats()
	{
		String json = "";
		return json;
	}
	
	private static String checkIfNumber(int col, String val)
	{
		// see if can parse as int. If can, return just value. if not, add "" around
		try 
		{ 
			Integer.parseInt(val);
			return val;
		}
		catch(Exception e)
			{ return '"' + val + '"'; }
	}
}
