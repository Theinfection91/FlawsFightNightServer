import fetch from 'node-fetch';
import { API_URL } from './creds.js';

export async function apiClient(endpoint, options = {}) {
    const response = await fetch(`${API_URL}${endpoint}`, {
        method: options.method || 'GET',
        headers: { 'Content-Type': 'application/json' },
        body: options.body ? JSON.stringify(options.body) : undefined
    });

    const raw = await response.text();
    let data;

    try {
        // Only parse JSON if body has content
        data = raw ? JSON.parse(raw) : {};
    } catch (err) {
        // Fallback to raw text if parsing fails
        data = raw;
    }

    if (!response.ok) {
        // Properly throw object for catching
        throw {
            status: response.status,
            body: data,
            message: data?.message || `API error: ${response.status}`
        };
    }

    return data;
}
