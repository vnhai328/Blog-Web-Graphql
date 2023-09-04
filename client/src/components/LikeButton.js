import React, { useContext, useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { useMutation } from "@apollo/react-hooks";
import gql from "graphql-tag"
import { Icon, Label, Button } from "semantic-ui-react";
import { AuthContext } from "../context/auth";
import MyPopup from "../util/MyPopup";
import { FETCH_POSTS_QUERY } from "../util/graphql";

const LikeButton = ({ post: { id, postLikes, likeCount } }) => {
    const { user } = useContext(AuthContext)
    const [liked, setLiked] = useState(false)
    const mutation = liked ? DISLIKE_POST_MUTATION : LIKE_POST_MUTATION 
    const likeId = postLikes[0]?.id
   
    useEffect(() => {
        if (user && postLikes.find((like) => like.creator.email === user.email)) {
            setLiked(true)
        }
        else {
            setLiked(false)
        }

    }, [user, postLikes])

    const [likePost] = useMutation(mutation, {
        variables: { 
            id: likeId,
            like_Id: id 
        },
        refetchQueries: [{query: FETCH_POSTS_QUERY}],

        onError({ networkError }) {
            
        },
    })
    
    
    const likeButton = user ? (
        liked ? (
            <Button color='red' onClick={() => likePost(likeId)}>
                <Icon name='heart' />
            </Button>
        ) : (
            <Button color='teal' basic>
                <Icon name='heart' />
            </Button>
        )
    ) : (
        <Button as={Link} to="/login" color='teal' basic>
            <Icon name='heart' />
        </Button>
    )

    return (
        <Button as='div' labelPosition='right' onClick={() => likePost()}>
            <MyPopup content={liked ? 'Unlike' : 'Like'}>
                {likeButton}
            </MyPopup>
            <Label as='a' basic color='red' pointing='left'>
                {likeCount}
            </Label>
        </Button>
    )
}

export default LikeButton

const LIKE_POST_MUTATION = gql`
    mutation ($like_Id: Int!){
        createLike(input: {
            likeId: $like_Id,
            like_Type: POST
        })
        {
        like{
            creatorId
            dateLiked
            like_Id
            like_Type
        }
        }
    }
`

const DISLIKE_POST_MUTATION = gql`
    mutation LikeInput($id: Int!){
        deleteLike(input: {
            id: $id
        })
        {
            boolean
        }
    }
`


