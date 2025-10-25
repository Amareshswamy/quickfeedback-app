document.addEventListener("DOMContentLoaded", () => {
    const feedbackForm = document.getElementById("feedback-form");
    const feedbackList = document.getElementById("feedback-list");
    const submitButton = document.getElementById("submit-button");
    const nameInput = document.getElementById("name");
    const messageInput = document.getElementById("message");

    const apiBaseUrl = "/api/feedback";

    /**
     * Fetches feedback entries from the API and renders them to the list.
     */
    async function loadFeedback() {
        feedbackList.innerHTML = '<p class="loading">Loading messages...</p>';
        try {
            const response = await fetch(apiBaseUrl);
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            const entries = await response.json();

            // Clear loading message
            feedbackList.innerHTML = "";

            if (entries.length === 0) {
                feedbackList.innerHTML = '<p class="loading">No messages yet. Be the first!</p>';
                return;
            }

            // Add each entry to the list
            entries.forEach(entry => {
                const item = document.createElement("article");
                item.className = "feedback-item";

                const header = document.createElement("div");
                header.className = "feedback-item-header";

                const name = document.createElement("strong");
                name.textContent = entry.name;

                const time = document.createElement("time");
                const date = new Date(entry.createdAt);
                time.textContent = date.toLocaleString();

                header.appendChild(name);
                header.appendChild(time);

                const message = document.createElement("p");
                message.textContent = entry.message;

                item.appendChild(header);
                item.appendChild(message);
                feedbackList.appendChild(item);
            });

        } catch (error) {
            console.error("Failed to load feedback:", error);
            feedbackList.innerHTML = '<p class="loading" style="color: red;">Error: Could not load messages.</p>';
        }
    }

    /**
     * Handles the form submission.
     */
    async function handleFormSubmit(event) {
        event.preventDefault(); // Stop the form from submitting normally

        submitButton.disabled = true;
        submitButton.textContent = "Submitting...";

        const newEntry = {
            name: nameInput.value,
            message: messageInput.value
        };

        try {
            const response = await fetch(apiBaseUrl, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(newEntry),
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            // Clear the form
            nameInput.value = "";
            messageInput.value = "";

            // Reload the feedback list
            await loadFeedback();

        } catch (error) {
            console.error("Failed to submit feedback:", error);
            alert("Error: Could not submit your message.");
        } finally {
            submitButton.disabled = false;
            submitButton.textContent = "Submit";
        }
    }

    // Attach event listeners
    feedbackForm.addEventListener("submit", handleFormSubmit);

    // Initial load
    loadFeedback();
});

