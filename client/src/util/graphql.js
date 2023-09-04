import gql from "graphql-tag"

export const FETCH_POSTS_QUERY = gql`
{
    posts(order: {
        createdDate: DESC
    }){
        creator{
            userId
            name
            email
            imageUri
        }
        id
        title
        content
        commmentCount
        comments{
            creator{
                userId
                name
                email
            }
            commentText
        }
        createdDate
        postLikes{
            id
            like_Id
            creator{
                userId
                name
                email
            }
        }
        likeCount
    }
}
`