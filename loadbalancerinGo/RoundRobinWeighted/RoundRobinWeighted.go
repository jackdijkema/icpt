package main

import (
	"fmt"
	"html"
	"io"
	"log"
	"net/http"
)

var servers = []Server{}

type Server struct {
	Port          string
	Weight        int
	CurrentWeight int
}

func main() {

	servers = []Server{
		{"3334", 10, 0},
		{"3335", 20, 0},
		{"3336", 10, 0},
	}

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

	var selected *Server = nil

	for i := range servers {

		servers[i].CurrentWeight += servers[i].Weight

		if selected == nil || servers[i].CurrentWeight > selected.CurrentWeight {
			selected = &servers[i]
		}

	}

	var target = "http://localhost:" + selected.Port + "/foo"

	selected.CurrentWeight -= selected.Weight

	resp, err := http.Get(target)

	if err != nil {
		log.Printf("Error: %v", err)
		http.Error(w, "Server unavailable", http.StatusInternalServerError)
		return
	}
	defer resp.Body.Close()
	io.Copy(w, resp.Body)
}

func handleResponse(w http.ResponseWriter, r *http.Request) {
	port := r.Host
	fmt.Fprintf(w, "Hello from %q on port %s\n", html.EscapeString(r.URL.Path), port)
	log.Default().Printf("Hello from %q on port %s\n", r.URL.Path, port)
}
