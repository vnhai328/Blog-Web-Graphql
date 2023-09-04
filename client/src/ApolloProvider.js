import { ApolloProvider, ApolloClient, InMemoryCache, from } from '@apollo/client';
import App from './App';
import { createHttpLink } from 'apollo-link-http';
import { onError } from "@apollo/client/link/error";
import React, { useState } from 'react';
import { createContext } from 'react';
import { setContext } from 'apollo-link-context';

export const ErrorContext = createContext();
function Provider() {
    const [errorMessage, setErrorMessage] = useState(null)

    const errorLink = onError(({ graphQLErrors, networkError }) => {
        if (graphQLErrors) {

            setErrorMessage(graphQLErrors[0].message)
            console.log(graphQLErrors[0].message)
        }
        else {
            setErrorMessage(null)
        }
        console.log(networkError)

    });

    const authLink = setContext(() => {
        const token = localStorage.getItem('jwtToken')
        // console.log(`Bearer ${token}`)
        return {
            headers: {
                Authorization: token ? `Bearer ${token}` : ''
            }
        }
        
        
    })

    const httpLink = createHttpLink({
        uri: 'http://localhost:5074/graphql'
    })

    const client = new ApolloClient({
        link: from([errorLink, authLink.concat(httpLink)]),
        cache: new InMemoryCache()
    });

    return (
        <ErrorContext.Provider value={errorMessage} >
            <ApolloProvider client={client}>
                <App />
            </ApolloProvider>
        </ErrorContext.Provider>
    )
}

export default Provider
