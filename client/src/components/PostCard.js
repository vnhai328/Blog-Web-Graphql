import React, { useContext } from "react";
import { Card, Icon, Label, Button, Image } from "semantic-ui-react";
import moment from "moment/moment";
import { Link } from "react-router-dom";
import { AuthContext } from "../context/auth";
import LikeButton from "./LikeButton";
import DeleteButton from "./DeleteButton";
import MyPopup from "../util/MyPopup";

function PostCard({ post: { id, title, content, commmentCount, createdDate, creator, likeCount, postLikes } }) {
    const { user } = useContext(AuthContext)

    return (
        <Card fluid>
            <Card.Content>
                <Image
                    floated='right'
                    size='mini'
                    src='https://react.semantic-ui.com/images/avatar/large/molly.png'
                />
                <Card.Header>{creator.name}</Card.Header>
                <Card.Meta as={Link} to={`/post/${id}`}>{moment(createdDate).fromNow(true)}</Card.Meta>
                <Card.Description>
                    {content}
                </Card.Description>
            </Card.Content>
            <Card.Content extra>
                <LikeButton user={user} post={{ id, postLikes, likeCount }} />
                <MyPopup content="Comment on post" >
                    <Button labelPosition='right' as={Link} to={`/post/${id}`}>
                        <Button color='blue'>
                            <Icon name='comments' />
                        </Button>
                        <Label as='a' basic color='blue' pointing='left'>
                            {commmentCount}
                        </Label>
                    </Button>
                </MyPopup>
                {user && user.email === creator.email && (<DeleteButton postId={id} />)}
            </Card.Content>
        </Card>
    )
}

export default PostCard