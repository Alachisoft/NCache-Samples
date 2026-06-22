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

package org.alachisoft.sessions_spring.controller;

import jakarta.servlet.http.HttpSession;
import org.alachisoft.sessions_spring.service.SessionService;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;

/**
 * Controller responsible for handling session-based user interactions.
 */
@Controller
@RequestMapping("/session")
public class SessionController {

    private final SessionService sessionService;

    /**
     * Creates a new {@code SessionController} with the given {@link SessionService}.
     *
     * @param sessionService service responsible for authentication and user management
     */
    public SessionController(SessionService sessionService) {
        this.sessionService = sessionService;
    }

    /**
     * Displays the session home page.
     * <p>
     * This method retrieves the session attributes and exposes them
     * to the view model for rendering.
     * </p>
     *
     * @param model   the Spring MVC model used to pass attributes to the view
     * @param session the current HTTP session
     * @return the name of the view to render
     */
    @GetMapping
    public String index(Model model, HttpSession session) {

        // pass relevant data to the UI for displaying fields
        model.addAttribute("authUser", session.getAttribute("authUser"));
        model.addAttribute("lastUsername", session.getAttribute("lastUsername"));
        model.addAttribute("loginFailed", session.getAttribute("loginFailed"));
        return "index";
    }

    /**
     * Handles user login requests.
     * <p>
     * Stores the last attempted username in the session and updates
     * session attributes based on authentication success or failure.
     * </p>
     *
     * @param username the username provided by the user
     * @param password the password provided by the user
     * @param session  the current HTTP session
     * @return a redirect to the session home page
     */
    @PostMapping("/login")
    public String login(@RequestParam String username,
                        @RequestParam String password,
                        HttpSession session) {

        // Store the last attempted username as a session attribute
        session.setAttribute("lastUsername", username);
        boolean success = sessionService.login(username, password);
        if (success) {
            // setting an authenticated user means the login passed
            session.setAttribute("authUser", username);
            session.setAttribute("loginFailed", false);
        } else {
            // authUser not set because login failed
            session.setAttribute("loginFailed", true);
        }

        return "redirect:/session";
    }

    /**
     * Handles new user registration requests.
     * <p>
     * Upon successful registration, the user is automatically authenticated
     * and relevant session attributes are initialized.
     * </p>
     *
     * @param username the username to register
     * @param password the password to register
     * @param session  the current HTTP session
     * @return a redirect to the session home page
     */
    @PostMapping("/register")
    public String register(@RequestParam String username,
                           @RequestParam String password,
                           HttpSession session) {

        sessionService.registerUser(username, password);

        // set login details as session attributes upon successful registration
        session.setAttribute("authUser", username);
        session.setAttribute("lastUsername", username);
        session.setAttribute("loginFailed", false);

        return "redirect:/session";
    }

    /**
     * Logs the current user out by removing authentication-related
     * attributes from the session.
     *
     * @param session the current HTTP session
     * @return a redirect to the session home page
     */
    @PostMapping("/logout")
    public String logout(HttpSession session) {

        // removing authUser from session means no one is logged in
        session.removeAttribute("authUser");
        return "redirect:/session";
    }
}
