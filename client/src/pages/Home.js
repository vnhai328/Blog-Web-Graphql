import React, { useContext } from "react";
import { useQuery } from "@apollo/react-hooks";
import { Card, Grid, Transition } from "semantic-ui-react";
import PostCard from '../components/PostCard'
import { AuthContext } from "../context/auth";
import PostForm from "../components/PostForm";
import { FETCH_POSTS_QUERY } from "../util/graphql";

function Home() {
    const { user } = useContext(AuthContext)
    
    const { loading, data } = useQuery(FETCH_POSTS_QUERY);

    const posts = data?.posts

    return (
        <Grid columns={3}>
            <Grid.Row className="page-title">
                <div>Post </div>
            </Grid.Row>
            <Grid.Row>
                {
                    user && (
                        <Grid.Column>
                            <PostForm />
                        </Grid.Column>
                    )
                }
                {loading ? (
                    <h1>Loading posts...</h1>
                ) : (
                    <Transition.Group>
                        {posts
                            && posts.map(post => (
                                <Grid.Column key={post.id} style={{ marginBottom: 20 }}>
                                    <Card.Group>
                                        <PostCard post={post} />
                                    </Card.Group>
                                </Grid.Column>
                            ))}
                    </Transition.Group>
                )}
            </Grid.Row>
        </Grid>
    )
}

export default Home