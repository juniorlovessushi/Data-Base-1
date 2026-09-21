// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

document.addEventListener("DOMContentLoaded", function () {

    // 1. Live Image File Preview for Event Banner Uploads
    const bannerInput = document.querySelector('input[type="file"][accept*="image"]');
    if (bannerInput) {
        bannerInput.addEventListener('change', function (event) {
            const file = event.target.files[0];
            if (file) {
                let previewImg = document.getElementById('banner-preview');

                // If preview element doesn't exist, create it dynamically below the input
                if (!previewImg) {
                    previewImg = document.createElement('img');
                    previewImg.id = 'banner-preview';
                    previewImg.className = 'img-fluid rounded mt-3 shadow-sm';
                    previewImg.style.maxHeight = '200px';
                    previewImg.style.width = '100%';
                    previewImg.style.objectFit = 'cover';
                    bannerInput.parentNode.appendChild(previewImg);
                }

                // Render selected file as preview image
                const reader = new FileReader();
                reader.onload = function (e) {
                    previewImg.src = e.target.result;
                };
                reader.readAsDataURL(file);
            }
        });
    }

});