import React, { useState } from "react";
import gql from "graphql-tag";
import { useMutation } from "@apollo/react-hooks";
import { Button, Confirm, Icon } from "semantic-ui-react";
import { FETCH_POSTS_QUERY } from "../util/graphql";
import MyPopup from "../util/MyPopup";


const DeleteButton = ({ postId, commentId, callback }) => {

    const [confirmOpen, setConfirmOpen] = useState(false)
    const mutation = commentId ? DELETE_COMMENT_MUTAION : DELETE_POST_MUTATION

    const [deletePostOrComment] = useMutation(mutation, {
        update(proxy) {
            setConfirmOpen(false)

            if (callback) callback()
        },
        variables: {
            postId,
            commentId
        },
        refetchQueries: [{query: FETCH_POSTS_QUERY}],
        onError(networkError) {

        }
    })

    return (
        <>
            <MyPopup content={commentId ? 'Delete comment' : 'Delete post'}>
                <Button
                    as="div"
                    color="red"
                    onClick={() => setConfirmOpen(true)}
                    floated="right"
                >
                    <Icon name="trash" style={{ margin: 0 }} />
                </Button>
            </MyPopup>

            <Confirm
                open={confirmOpen}
                onCancel={() => setConfirmOpen(false)}
                onConfirm={deletePostOrComment}
            />
        </>
    )
}

const DELETE_POST_MUTATION = gql`
    mutation DeletePost($postId: Int!) {
        deletePost(input: {
            id: $postId
        })
        {
            boolean
        }
    }
`

const DELETE_COMMENT_MUTAION = gql`
    mutation DeleteCommmentInput($commentId: Int!) {
        deleteComment(input: {
            id: $commentId
        })
        {
            boolean
        }
}
`

export default DeleteButton