package main

import (
	"fmt"
	"html"
	"io"
	"log"
	"net/http"
)

var servers = []string{}
var counter = 0

func main() {

	servers = []string{"3334", "3335", "3336"}

	mux1 := http.NewServeMux()
	mux2 := http.NewServeMux()
	mux3 := http.NewServeMux()

	mux := http.NewServeMux()

	mux.HandleFunc("/foo", handleRequest)

	mux1.HandleFunc("/foo", handleResponse)
	mux2.HandleFunc("/foo", handleResponse)
	mux3.HandleFunc("/foo", handleResponse)

	go func() {
		log.Println("Starting server on :3334 (mux1)")
		http.ListenAndServe(":3334", mux1)
	}()

	go func() {
		log.Println("Starting server on :3335 (mux2)")
		http.ListenAndServe(":3335", mux2)
	}()

	go func() {
		log.Println("Starting server on :3336 (mux3)")
		http.ListenAndServe(":3336", mux3)
	}()

	log.Print("Main server...(mux)")

	http.ListenAndServe(":3333", mux)

}

func handleRequest(w http.ResponseWriter, r *http.Request) {
	if counter >= len(servers) {
		counter = 0
	}
	var target = "http://localhost:" + servers[counter] + "/foo"
	resp, err := http.Get(target)

	if err != nil {
		log.Printf("Error: %v", err)
		http.Error(w, "Server unavailable", http.StatusInternalServerError)
		return
	}
	defer resp.Body.Close()
	counter++

	io.Copy(w, resp.Body)
}

func handleResponse(w http.ResponseWriter, r *http.Request) {
	port := r.Host
	fmt.Fprintf(w, "Hello from %q on port %s\n", html.EscapeString(r.URL.Path), port)
}
