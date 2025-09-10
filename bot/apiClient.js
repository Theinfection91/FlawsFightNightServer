import fetch from 'node-fetch';
import { API_URL } from './creds.js';

//import dotenv from 'dotenv';
//dotenv.config();

console.log(`API_URL is set to: ${API_URL}`);

export async function apiClient(endpoint, options = {}) {
    const res = await fetch(`${API_URL}${endpoint}`, {
        method: options.method || 'GET',
        headers: { 'Content-Type': 'application/json' },
        body: options.body ? JSON.stringify(options.body) : undefined
    });

    const data = await res.json();

    if (!res.ok) {
        // Attach status to the error
        const error = new Error(data?.message || `API error: ${res.status}`);
        error.status = res.status;
        throw error;
    }

    return data;
}