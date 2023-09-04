import React, { useContext, useState } from 'react'
import { Menu } from 'semantic-ui-react'
import { Link } from 'react-router-dom'
import 'semantic-ui-css/semantic.min.css';

import { AuthContext } from '../context/auth';

function MenuBar() {
    const { user, logout } = useContext(AuthContext)
    const pathName = window.location.pathname
    const path = pathName === '/' ? 'home' : pathName.substr(1);
    const [activeItem, setActiveItem] = useState(path)
    const handleItemClick = (e, { name }) => setActiveItem(name)
    var setName = useState('')
    if(user != null)
    {
        if(!user.name)
        {
            setName = user.unique_name
        }
        else if(!user.unique_name)
        {
            setName = user.name
        }
    } else {
        console.log('Damn! you not login and wtf you find here?')
    }

    const menuBar = user ? (
        <Menu pointing secondary size='massive' color='teal'>
            <Menu.Item
                name={setName}
                active
                as={Link}
                to="/"
            />
            <Menu.Menu position='right'>
                <Menu.Item
                    name='logout'
                    active
                    onClick={logout}
                    as={Link}
                />
            </Menu.Menu>
        </Menu>
    ) : (
        <Menu pointing secondary size='massive' color='teal'>
            <Menu.Item
                name='home'
                active={activeItem === 'home'}
                onClick={handleItemClick}
                as={Link}
                to="/"
            />
            <Menu.Menu position='right'>
                <Menu.Item
                    name='login'
                    active={activeItem === 'login'}
                    onClick={handleItemClick}
                    as={Link}
                    to="/login"
                />

                <Menu.Item
                    name='register'
                    active={activeItem === 'register'}
                    onClick={handleItemClick}
                    as={Link}
                    to="register"
                />
            </Menu.Menu>
        </Menu>
    )

    return menuBar
}

export default MenuBar