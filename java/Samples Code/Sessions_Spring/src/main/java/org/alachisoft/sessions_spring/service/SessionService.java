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

package org.alachisoft.sessions_spring.service;

import org.alachisoft.sessions_spring.repository.SpringSessionRepository;
import org.springframework.stereotype.Service;

/**
 * Service layer responsible for handling user authentication
 * and registration logic.
 */
@Service
public class SessionService {

    private final SpringSessionRepository springSessionRepository;

    /**
     * Creates a new {@code SessionService} with the given repository.
     *
     * @param springSessionRepository repository responsible for session handling
     */
    public SessionService(SpringSessionRepository springSessionRepository) {
        this.springSessionRepository = springSessionRepository;
    }

    /**
     * Registers a new user with the provided credentials.
     *
     * @param username the username to register
     * @param password the password associated with the user
     */
    public void registerUser(String username, String password) {
        springSessionRepository.registerUser(username, password);
    }

    /**
     * Authenticates a user using the provided credentials.
     *
     * @param username the username to authenticate
     * @param password the user's password
     * @return {@code true} if authentication is successful; {@code false} otherwise
     */
    public boolean login(String username, String password) {
        return springSessionRepository.login(username, password);
    }
}
