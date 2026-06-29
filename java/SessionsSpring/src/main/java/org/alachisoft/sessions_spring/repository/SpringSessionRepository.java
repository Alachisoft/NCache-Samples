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

package org.alachisoft.sessions_spring.repository;

import org.springframework.stereotype.Repository;

import java.util.HashMap;

/**
 * Repository responsible for managing user credentials. Only
 * an in memory implementation for demonstration purposes.
 */
@Repository
public class SpringSessionRepository {

    /**
     * In-memory storage for user credentials, keyed by username.
     */
    private final HashMap<String, String> credentials = new HashMap<>();

    /**
     * Registers a new user by storing the provided credentials.
     *
     * @param username the username to register
     * @param password the password associated with the user
     */
    public void registerUser(String username, String password) {
        credentials.put(username, password);
    }

    /**
     * Authenticates a user by validating the provided credentials
     * against the stored values.
     *
     * @param username the username attempting to authenticate
     * @param password the password provided by the user
     * @return {@code true} if the credentials are valid; {@code false} otherwise
     */
    public boolean login(String username, String password) {
        if (credentials.containsKey(username)) {
            return credentials.get(username).equals(password);
        }
        return false;
    }
}
