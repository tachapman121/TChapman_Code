package tests;

import java.util.*;

/**
 * PROBLEM: 
 * 
 * Harold is a kidnapper who wrote a ransom note, but now he is worried it will be traced back to him through his handwriting. 
 * He found a magazine and wants to know if he can cut out whole words from it and use them to create an untraceable replica of his ransom note. 
 * The words in his note are case-sensitive and he must use only whole words available in the magazine. 
 * He cannot use substrings or concatenation to create the words he needs.
 * Given the words in the magazine and the words in the ransom note, print Yes if he can replicate his ransom note exactly using whole words 
 * from the magazine; otherwise, print No.
 * 
 * Words are case sensitive
 */
public class RansomNote {
    private static final Scanner scanner = new Scanner(System.in);

    /**
     * Provided MAIN function
     * 
     * @param args
     * First line should be 2 space separated integers: number of words in magazine and number of words in notes
     * Second should be strings in magazine separated by spaces
     * Third should be number of strings in note separated by spaces  
     */
    public static void main(String[] args) {
        String[] mn = scanner.nextLine().split(" ");

        int m = Integer.parseInt(mn[0]);

        int n = Integer.parseInt(mn[1]);

        String[] magazine = new String[m];

        String[] magazineItems = scanner.nextLine().split(" ");
        scanner.skip("(\r\n|[\n\r\u2028\u2029\u0085])?");

        for (int i = 0; i < m; i++) {
            String magazineItem = magazineItems[i];
            magazine[i] = magazineItem;
        }

        String[] note = new String[n];

        String[] noteItems = scanner.nextLine().split(" ");
        scanner.skip("(\r\n|[\n\r\u2028\u2029\u0085])?");

        for (int i = 0; i < n; i++) {
            String noteItem = noteItems[i];
            note[i] = noteItem;
        }

        checkMagazine(magazine, note);

        scanner.close();
    }
    
    static void checkMagazine(String[] magazine, String[] note) {
        Hashtable<String, Integer> table = new Hashtable<String, Integer>();

        // get list of magazine words
        for (int i = 0; i < magazine.length; i++) {
            String word = magazine[i];
            Integer val = table.get(word);
            if (val != null) {
                table.put(word, ++val);
            } else
                table.put(word, 1);
        }

        // check notes
        for (int i = 0; i < note.length; i++) {
            String noteWord = note[i];
            Integer val = table.get(noteWord);
            
            // if word does not exist or are out of words, fail the test
            if (val != null) {
                if (val == 0) {
                    System.out.print("No"); // fail
                    return;
                }
                val--;
                table.put(noteWord, val);
            } else {
                    System.out.print("No"); // fail
                    return;
            }
        }

        // if reaches this point, then words in notes are <= words in magazine, success
        System.out.print("Yes"); 
    }
}
