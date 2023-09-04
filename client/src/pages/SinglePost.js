import gql from "graphql-tag";
import React, { useContext, useRef, useState } from "react";
import { useMutation, useQuery } from "@apollo/react-hooks";
import { Button, Card, Form, Grid, Icon, Image, Label } from "semantic-ui-react";
import moment from "moment/moment";
import LikeButton from "../components/LikeButton";
import { AuthContext } from "../context/auth";
import DeleteButton from "../components/DeleteButton";
import { useParams } from "react-router-dom";
import { useNavigate } from "react-router-dom";
import MyPopup from "../util/MyPopup";
import { FETCH_POSTS_QUERY } from "../util/graphql";

const SinglePost = () => {
    const { postId } = useParams()
    const id = parseInt(postId, 10)
    const navigate = useNavigate()
    const [addComment, setAddComment] = useState('')
    const { user } = useContext(AuthContext)
    const commentInputRef = useRef(null)

    const { data } = useQuery(FETCH_POST_QUERY, {
        variables: {
            id: id
        },

        onError(networkError) {

        }
    })

    const [submmitComment] = useMutation(SUBMIT_COMMENT_MUTATION, {
        update() {
            setAddComment('')
            commentInputRef.current.blur()
        },

        onError(networkError) {

        },

        variables: {
            comment: addComment,
            postId: id
        },
        refetchQueries: [{query: FETCH_POSTS_QUERY}],
    })

    const deletePostCallback = () => {
        navigate('/')
    }

    const postById = data?.postById
    console.log(postById)
    let postMarkup;
    if (!postById) {
        postMarkup = <p>Loading Post...</p>
    } else {
        const { content, commmentCount, createdDate, creator, likeCount, postLikes, comments } = postById
        postMarkup = (
            <Grid>
                <Grid.Row>
                    <Grid.Column width={2}>
                        <Image
                            floated='right'
                            size='small'
                            src='https://react.semantic-ui.com/images/avatar/large/molly.png'
                        />
                    </Grid.Column>
                    <Grid.Column width={10}>
                        <Card fluid>
                            <Card.Content >
                                <Card.Header>{creator.name}</Card.Header>
                                <Card.Meta>{moment(createdDate).fromNow(true)}</Card.Meta>
                                <Card.Description>{content}</Card.Description>
                                <hr />
                                <Card.Content extra>
                                    <LikeButton user={user} post={{ id, likeCount, postLikes }} />
                                    <MyPopup content="Comment on post">
                                        <Button
                                            as="div"
                                            labelPosition="right"
                                            onClick={() => console.log('Commment on post')}
                                        >
                                            <Button basic color="blue">
                                                <Icon name="comments" />
                                            </Button>
                                            <Label basic color="blue" pointing="left">
                                                {commmentCount}
                                            </Label>
                                        </Button>
                                    </MyPopup>
                                    {user && user.email === creator.email && (
                                        <DeleteButton postId={id} callback={deletePostCallback} />
                                    )}
                                </Card.Content>
                            </Card.Content>
                        </Card>
                        {user && (
                            <Card fluid>
                                <Card.Content>
                                    <p>Post Comment</p>
                                    <Form>
                                        <div className="ui action input fluid">
                                            <input
                                                type="text"
                                                placeholder="Comment..."
                                                name="addComment"
                                                value={addComment}
                                                onChange={(event) => setAddComment(event.target.value)}
                                                ref={commentInputRef}
                                            />
                                            <button
                                                type="submit"
                                                className="ui button teal"
                                                disabled={addComment.trim() === ''}
                                                onClick={submmitComment}
                                            >
                                                Submit
                                            </button>
                                        </div>
                                    </Form>
                                </Card.Content>
                            </Card>
                        )}
                        {comments.map(comment => (
                            <Card fluid key={comment.id}>
                                <Card.Content>
                                    {user && user.email === comment.creator.email && (
                                        <DeleteButton postId={id} commentId={comment.id} />
                                    )}
                                    <Card.Header>{comment.creator.name}</Card.Header>
                                    <Card.Meta>{moment(comment.dateCommented).fromNow(true)}</Card.Meta>
                                    <Card.Description>{comment.commentText}</Card.Description>
                                </Card.Content>
                            </Card>
                        ))}
                    </Grid.Column>
                </Grid.Row>
            </Grid>
        )
        return postMarkup
    }
}

export default SinglePost

const SUBMIT_COMMENT_MUTATION = gql`
    mutation commentInput($comment: String!, $postId: Int!){
        createComment (input: {
            commentInput: {
                comment: $comment
            }
            postId: $postId
        })
        {
            commentPayLoad{
                comment{
                    id
                    creatorId
                    commentText
                    dateCommented
                    postId
                }
            }
        }
        }
`

const FETCH_POST_QUERY = gql`
    query ($id: Int!) {
        postById(id: $id) {
            id
            title
            content
            creator {
                userId
                email
                name
                imageUri
            }
            commmentCount
            comments {
                id
                creator {
                    userId
                    name
                    email
                }
                commentText
                dateCommented
            }
            createdDate
            postLikes {
                id
                like_Id
                creator {
                    userId
                    name
                    email
                }
            }
            likeCount
        }
    }
`