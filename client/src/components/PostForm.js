import React, { useContext } from "react";
import { Button, Form } from "semantic-ui-react";
import { useFrom } from "../util/hook";
import gql from "graphql-tag";
import { useMutation } from "@apollo/react-hooks";
import { ErrorContext } from "../ApolloProvider";
import { FETCH_POSTS_QUERY } from "../util/graphql";

function PostForm() {
    const errorMessage = useContext(ErrorContext)
    const creatPostCallBack = () => {
        createPost()
        
 
    }

    const { values, onChange, onSubmit } = useFrom(creatPostCallBack, {
        title: 'POST',
        content: ''

    })

    const [createPost] = useMutation(CREATE_POST_MUTATION, {
        variables: values,
        refetchQueries: [{query: FETCH_POSTS_QUERY}],
        update(proxy, result) {

            if(result){
               
            }

            values.content = ''
        },

        onError({ networkError }) {
            
        },
    })



    return (
        <>
            <Form onSubmit={onSubmit}>
                <h2>Create a post: </h2>
                <Form.Field>
                    <Form.Input
                        placceholder="Hi"
                        name="content"
                        onChange={onChange}
                        value={values.content}
                    />
                    <Button type="submit" color="teal">
                        Submit
                    </Button>
                </Form.Field>
            </Form>
            {
                errorMessage &&
                <div className="ui error message" style={{marginBottom: 20}}>
                    <ul className="list">
                        <li>{errorMessage}</li>
                    </ul>
                </div>
            }
        </>
    )
}

const CREATE_POST_MUTATION = gql`
    mutation PostInput($title: String!, $content: String!) {
        createPost(input: {
            input: {
                title: $title,
                content: $content
            }
        })
        {
            postPayload{
                post{
                    id
                    title
                    content
                    createdDate
                }
            }
        }
}

`

export default PostForm