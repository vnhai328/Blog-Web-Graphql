import gql from "graphql-tag";
import React, { useContext } from "react";
import { useMutation } from "@apollo/react-hooks";
import { Button, Form } from "semantic-ui-react";
import { ErrorContext } from "../ApolloProvider";
import { useNavigate } from "react-router-dom";
import { useFrom } from "../util/hook";
import { AuthContext } from "../context/auth";

function Register(props) {
    const context = useContext(AuthContext)
    const errorMessage = useContext(ErrorContext)
    const navigate = useNavigate()

    const { onChange, onSubmit, values } = useFrom(registerUser, {
        name: '',
        email: '',
        password: '',
    })

    const [addUser, { loading }] = useMutation(REGISTER_USER, {
        update(_, { data: { register: userData } }) {
            context.login(userData)
            navigate('/')
        },

        onError({ networkError }) {

        },
        variables: values
    })

    function registerUser() {
        addUser();
    }

    return (
        <div className="form-container">
            <Form onSubmit={onSubmit} noValidate className={loading ? 'loading' : ''}>
                <h1>Register</h1>
                <Form.Input
                    label="Username"
                    placeholder="Username..."
                    name="name"
                    type="text"
                    value={values.name}
                    onChange={onChange}
                />
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
                    Register
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

const REGISTER_USER = gql`
    mutation SignUpInput(
        $name: String!, 
        $email: String!, 
        $password: String!
        ) 
        {
            signUp(input: { 
                name: $name, 
                email: $email, 
                password: $password 
                }) 
                {
                signUpPayLoad 
                {
                    user {
                        id
                        person {
                            userId
                            name
                            lastSeen
                        }
                        email
                    }
                }
            }
        }
    `

export default Register