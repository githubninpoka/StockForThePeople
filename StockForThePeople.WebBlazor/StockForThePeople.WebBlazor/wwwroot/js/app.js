function userScroll() {
    const navbar = document.querySelector('.navbar');

    window.addEventListener('scroll', () => {
        if (window.scrollY > 50) {
            navbar.classList.add('bg-dark');
            // see custom style
            navbar.classList.add('navbar-sticky');
        } else {
            navbar.classList.remove('bg-dark');
            navbar.classList.remove('navbar-sticky');
        }
    })
}

function menuWatch() {
    const navbarButton = document.querySelector('.navbar-toggler');
    const navbar = document.querySelector('.navbar');

    navbarButton.addEventListener('click', () => {
        if (!navbarButton.classList.contains('collapsed')) {
            navbar.classList.add('bg-dark');
        } else {
            if (window.scrollY < 51) {
                navbar.classList.remove('bg-dark');
            }
        }
    })
}