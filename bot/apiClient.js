import fetch from 'node-fetch';
import { API_URL } from './creds.js';

//import dotenv from 'dotenv';
//dotenv.config();

console.log(`API_URL is set to: ${API_URL}`);

export async function apiClient(endpoint, options = {}) {
    const response = await fetch(`http://localhost:5000/api${endpoint}`, {
        method: options.method || 'GET',
        headers: { 'Content-Type': 'application/json' },
        body: options.body ? JSON.stringify(options.body) : undefined
    });

    const raw = await response.text(); // only once
    let data;

    try {
        data = JSON.parse(raw);
    } catch {
        data = raw; // plain text fallback
    }

    if (!response.ok) {
        const err = new Error(`API error: ${response.status}`);
        err.status = response.status;
        err.body = data;
        throw err;
    }

    return data;
}
