package guessgame;

import java.util.Random;
import java.io.Serializable;
import java.util.ArrayList;

public class GuessGame implements Serializable
{
    private ArrayList<Integer> history = new ArrayList<Integer>();
    private int secretNumber = 0;
    private int maxUpperLimit = 101;
    private int min = 0;
    private int max = 101;

    //--------------------------------------------------------------------------
    public GuessGame()
    {
        initialize();
    }
    //--------------------------------------------------------------------------
    public void initialize()
    {
        Random rand = new Random();
        min = 0;
        max = maxUpperLimit;
        secretNumber = rand.nextInt(maxUpperLimit);
    }
    //--------------------------------------------------------------------------
    public boolean makeGuess(int guess)
    {
        history.add(guess);
        if (guess == secretNumber)
        {
            return true;
        }
        return false;
    }
    //--------------------------------------------------------------------------
    public void newGame()
    {
        history.clear();
        initialize();
    }
    //--------------------------------------------------------------------------
    public String getHistory()
    {
        String histText = "";
        for (int i = 0; i < history.size(); i++)
        {
            if (i > 0)
            {
                histText += ", ";
            }
            histText += history.get(i);
        }
        return histText;
    }
    //--------------------------------------------------------------------------
    public int getGuessCount()
    {
        return history.size();
    }
    //--------------------------------------------------------------------------
    public String getRetryInfo()
    {
        String message = "<p>";

        if (getGuessCount() == 0)
        {
            message = "You have not attempted yet.";
        }
        else
        {
            int num = getLastGuess();
            if (num < secretNumber)
            {
                message = "Attempt #" + getGuessCount() + " - The number that you have tried is SMALLER than the guess.</p>";
            }
            else if (num > secretNumber)
            {
                message = "Attempt #" + getGuessCount() + " - The number that you have tried is GREATER than the guess.</p>";
            }
        }
        message += "</p>";
        return message;
    }
    //--------------------------------------------------------------------------
    public String getHint()
    {
        String hint = "<p class=\"hint\">Hint: Number is between " + min + " and " + max + "</p>";
        if (getGuessCount() > 0)
        {
            int num = getLastGuess();
            if (num < secretNumber && num > min)
            {
                min = num;
            }
            if (num > secretNumber && num < max)
            {
                max = num;
            }

            if (num == secretNumber)
            {
                hint = "<p class=\"success\">Congratulations! You have found the Secret Number.</p>";
            }
            else
            {
                hint = "<p class=\"hint\">Hint: Number is between " + min + " and " + max + "</p>";
            }
        }
        return hint;
    }
    //--------------------------------------------------------------------------
    public int getLastGuess()
    {
        if (getGuessCount() == 0)
        {
            return -1;
        }
        else
        {
            return history.get(history.size() - 1);
        }
    }
    //--------------------------------------------------------------------------
}
