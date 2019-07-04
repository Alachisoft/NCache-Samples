<%@page contentType="text/html" pageEncoding="UTF-8" %>
<%@page import="guessgame.GuessGame" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html>
    <head>
        <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
        <LINK REL=StyleSheet HREF="GuessGame.css" TYPE="text/css">
        <title>Guess Game Session Provider</title>
    </head>
    <body>
        <%
        GuessGame game = (GuessGame)session.getAttribute("game");
        if (game == null)
        {
            game = new GuessGame();
            session.setAttribute("game", game);
        }

        String param = request.getParameter("txtGuess");
        String errorMsg = "";
        boolean guessed = false;
        try
        {
            if (param != null && param.compareTo("") != 0)
            {
                int guess = Integer.parseInt(param);
                guessed = game.makeGuess(guess);
            }
        }
        catch(Exception exp)
        {
            errorMsg = "Invalid number entered.";
        }
        %>
        <table class="wrapper">
            <caption><p>Using NCache's Session Store Provider Implementation<p></caption>
            <tr>
                <td>
                    <table>
                        <tr>
                            <th>
                                The computer has selected a random number between 1 and 100 (inclusive).<br>In this game you have to try to guess the number.
                            </th>
                        </tr>
                        <tr>
                            <td>
                                <p class="error"><%=errorMsg %></p>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <br>
                                <form method="post" action="">
                                    Enter a number:&nbsp;
                                    <input type="text" name="txtGuess" value="" maxlength="3" />
                                    <input type="submit" value="<%=guessed ? "New Game" : "Guess" %>" />
                                </form>
                                <br><br><br>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%=game.getRetryInfo()%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%=game.getHint()%>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Attempts History:
                            </th>
                        </tr>
                        <tr>
                            <td>
                                <br>Last guess: &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <%=game.getLastGuess()==-1 ? "No guess yet" : "" + game.getLastGuess()%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Previous Attempts: <%=game.getHistory()%>
                                <br><br>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Page generated at : <%=request.getServerName()%>
                            </th>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </body>
    <%
        if(guessed)
        {
            game.newGame();
        }
    %>
</html>
