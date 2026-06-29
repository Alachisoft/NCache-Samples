/*
 * ===============================================================================
 * Alachisoft (R) NCache Sample Code.
 * NCache Basic Operations sample
 * ===============================================================================
 * Copyright © Alachisoft.  All rights reserved.
 * THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
 * OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
 * LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
 * FITNESS FOR A PARTICULAR PURPOSE.
 * ===============================================================================
 */

package main.java.com.alachisoft.ncache.sample;

import java.util.Random;
import java.io.Serializable;
import java.util.ArrayList;

/*
 * This class is used in an NCache HTTP session-based sample application.
 *
 * NCache sessions allow application state to be stored outside the web server
 * in a distributed cache. This enables session data to survive application
 * restarts and be shared across multiple server instances.
 *
 * The GuessGame object is stored as a session attribute inside NCache.
 * On each user request, the object is retrieved from the session, updated
 * based on user input, and stored back into the cache.
 */

public class GuessGame implements Serializable {

    private ArrayList<Integer> history = new ArrayList<Integer>();
    private int secretNumber = 0;
    private int maxUpperLimit = 101;
    private int min = 0;
    private int max = 101;

    /**
     * Creates a new {@code GuessGame} and initializes the game state.
     */
    public GuessGame() {
        initialize();
    }

    /**
     * Initializes the game by generating a new secret number
     * and resetting the guess range.
     */
    public void initialize() {
        Random rand = new Random();
        min = 0;
        max = maxUpperLimit;
        secretNumber = rand.nextInt(maxUpperLimit);
    }

    /**
     * Processes a user guess and records it in the guess history.
     *
     * @param guess the number guessed by the user
     * @return {@code true} if the guess matches the secret number;
     *         {@code false} otherwise
     */
    public boolean makeGuess(int guess) {
        history.add(guess);
        return guess == secretNumber;
    }

    /**
     * Starts a new game by clearing the guess history
     * and generating a new secret number.
     */
    public void newGame() {
        history.clear();
        initialize();
    }

    /**
     * Returns the guess history as a comma-separated string.
     *
     * @return a string containing all previous guesses
     */
    public String getHistory() {
        String histText = "";
        for (int i = 0; i < history.size(); i++) {
            if (i > 0) {
                histText += ", ";
            }
            histText += history.get(i);
        }
        return histText;
    }

    /**
     * Returns the total number of guesses made so far.
     *
     * @return the number of guesses
     */
    public int getGuessCount() {
        return history.size();
    }

    /**
     * Provides feedback based on the user's last guess,
     * indicating whether the guess was higher or lower
     * than the secret number.
     *
     * @return a formatted message describing the result
     *         of the most recent guess
     */
    public String getRetryInfo() {
        String message = "<p>";

        if (getGuessCount() == 0) {
            message = "You have not attempted yet.";
        } else {
            int num = getLastGuess();
            if (num < secretNumber) {
                message = "Attempt #" + getGuessCount()
                        + " - The number that you have tried is SMALLER than the guess.</p>";
            } else if (num > secretNumber) {
                message = "Attempt #" + getGuessCount()
                        + " - The number that you have tried is GREATER than the guess.</p>";
            }
        }
        message += "</p>";
        return message;
    }

    /**
     * Returns a hint message showing the valid range
     * for the secret number based on previous guesses.
     *
     * @return a formatted hint or success message
     */
    public String getHint() {
        String hint = "<p class=\"hint\">Hint: Number is between " + min + " and " + max + "</p>";

        if (getGuessCount() > 0) {
            int num = getLastGuess();

            if (num < secretNumber && num > min) {
                min = num;
            }
            if (num > secretNumber && num < max) {
                max = num;
            }

            if (num == secretNumber) {
                hint = "<p class=\"success\">Congratulations! You have found the Secret Number.</p>";
            } else {
                hint = "<p class=\"hint\">Hint: Number is between " + min + " and " + max + "</p>";
            }
        }
        return hint;
    }

    /**
     * Returns the most recent guess made by the user.
     *
     * @return the last guessed number, or {@code -1} if no guesses exist
     */
    public int getLastGuess() {
        if (getGuessCount() == 0) {
            return -1;
        } else {
            return history.get(history.size() - 1);
        }
    }
}

