import gql from "graphql-tag";
import React, { useContext } from "react";
import { useMutation } from "@apollo/react-hooks";
import { Button, Form } from "semantic-ui-react";
import { ErrorContext } from "../ApolloProvider";
import { useNavigate } from "react-router-dom";
import { useFrom } from "../util/hook";
import { AuthContext } from "../context/auth";

function Login() {
    const context = useContext(AuthContext)
    const errorMessage = useContext(ErrorContext)
    const navigate = useNavigate()

    const { onChange, onSubmit, values } = useFrom(loginUserCalback, {
        email: '',
        password: '',
    })

    const [loginUser, { loading }] = useMutation(LOGIN_USER, {
        update(_, {data: {login: userData}}) {
            context.login(userData)
            navigate('/')
        },

        onError({ networkError }) {

        },
        variables: values
    })

    function loginUserCalback() {
        loginUser();
    }

    return (
        <div className="form-container">
            <Form onSubmit={onSubmit} noValidate className={loading ? 'loading' : ''}>
                <h1>Login</h1>
                <Form.Input
                    label="Email"
                    placeholder="Email..."
                    name="email"
                    type="text"
                    value={values.email}
                    onChange={onChange}
                />
                <Form.Input
                    label="Password"
                    placeholder="Password..."
                    name="password"
                    type="password"
                    value={values.password}
                    onChange={onChange}
                />
                <Button type="submit" primary>
                    Login
                </Button>
            </Form>
            {
                errorMessage &&
                <div className="ui error message">
                    <ul className="list">
                        <div>{errorMessage}</div>
                    </ul>
                </div>
            }
        </div>
    )
}

export default Login

const LOGIN_USER = gql`
    mutation LoginInput(
        $email: String!, 
        $password: String!
        ) 
        {
            login(input: { 
                email: $email, 
                password: $password 
                }) 
                {
                    loginPayLoad 
                    {
                        me {
                            email
                            name
                            lastSeen
                            imageUri
                        }
                        schema
                        token
                    }
                }
        }
    `