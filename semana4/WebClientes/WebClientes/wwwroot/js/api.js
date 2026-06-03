const API_BASE = '/api';

async function fetchApi(url, method = 'GET', body = null) {
    const options = {
        method,
        headers: {
            'Content-Type': 'application/json'
        }
    };
    
    if (body) {
        options.body = JSON.stringify(body);
    }

    try {
        const response = await fetch(`${API_BASE}${url}`, options);
        if (!response.ok) {
            if (response.status === 404) return null;
            if (response.status === 204) return true;
            throw new Error(`Error: ${response.status} ${response.statusText}`);
        }
        
        // 204 No Content
        if (response.status === 204) return true;

        return await response.json();
    } catch (error) {
        console.error('Fetch error:', error);
        throw error;
    }
}
