
const footer = {
    informations: [
        {
            label: "Abous Us",
            link: "/About"
        },
        {
            label: "Delivery Information",
            link: "/Delivery-Information"
        },
        {
            label: "Privacy Policy",
            link: "/Privacy-Policy"
        },
        {
            label: "Terms & Conditions",
            link: "/Terms-Conditions"
        },
        {
            label: "Customer Service",
            link: "/Customer-Service"
        }
    ],
    accounts: [
        {
            label: "My Account",
            link: "/Account",
        },
        {
            label: "Order History",
            link: "/Order"
        },
        {
            label: "Wish List",
            link: "/Wishlist"
        },
        {
            label: "Newsletter",
            Link: "/Newsletter"
        },
        {
            label: "Order History",
            link: "/Order"
        }
    ],
    addresses: [
        {
            label: "Address",
            content: "Le Trong Tan, Ha Dong, Ha Noi"
        },
        {
            label: "Telephone Enquiry",
            content: "(012) 345 6789"
        },
        {
            label: "Email",
            content: "deme@example.com"
        }
    ],
};


footer.informations.forEach((information) => {
    $(".footer-information ul").append(`
    <li>
        <a href=${information.link}>${information.label}</a>
    </li >`)
})


footer.accounts.forEach((account) => {
    $(".footer-account ul").append(`
    <li>
        <a href=${account.link}>${account.label}</a>
    </li>`)
})


footer.addresses.forEach((address) => {
    $(".footer-address ul").append(`
        <li>
            <span>${address.label}:</span>
            ${address.content}
        </li>
    `)
})


