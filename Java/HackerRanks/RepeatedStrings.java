package tests;
import java.io.*;
import java.util.*;

/**
 * PROBLEM: 
 * Lilah has a string, s, of lowercase English letters that she repeated infinitely many times.
 * Given an integer, n, find and print the number of letter a's in the first  letters of Lilah's infinite string.
 */
public class RepeatedStrings {
    static long repeatedString(String s, long n) {
        long aCount = 0;

        for(int i = 0; i < s.length(); i++) {
            if (s.charAt(i) == 'a')
                aCount++;
        }

        long mult = n / s.length();
        aCount = mult * aCount;

        // wrapup
        long wrapup = n % s.length();
        for (int i = 0; i < wrapup; i++) {
            if(s.charAt(i) == 'a')
                aCount++;
        }

        return aCount;
    }

    private static final Scanner scanner = new Scanner(System.in);

    /**
     * @param args First line is string in form 'aba', second line is number of letters to search for
     * @throws IOException
     */
    
    // example input:
    // ababbab
    // 20
    public static void main(String[] args) throws IOException {

        String s = scanner.nextLine();
        long n = scanner.nextLong();
        scanner.skip("(\r\n|[\n\r\u2028\u2029\u0085])?");

        long result = repeatedString(s, n);
        System.out.println(result);

        scanner.close();
    }
}
