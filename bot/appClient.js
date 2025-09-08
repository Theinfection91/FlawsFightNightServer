import fetch from 'node-fetch';
import { API_URL } from './creds.js';

//import dotenv from 'dotenv';
//dotenv.config();

console.log(`API_URL is set to: ${API_URL}`);

export async function apiClient(endpoint, options = {}) {
    const res = await fetch(`${API_URL}${endpoint}`, {
        method: options.method || 'GET',
        headers: {
            'Content-Type': 'application/json',
        },
        body: options.body ? JSON.stringify(options.body) : undefined
    });

    if (!res.ok) {
        throw new Error(`API error: ${res.status} ${res.statusText}`);
    }

    return res.json();
}