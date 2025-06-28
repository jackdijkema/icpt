package main

import (
	"fmt"
	"html"
	"io"
	"log"
	"net/http"
	"sync"
)

var servers = []Server{}

type Server struct {
	Port        string
	Connections int
	Mutex       sync.Mutex
}

func main() {

	servers = []Server{
		{"3334", 0, sync.Mutex{}},
		{"3335", 0, sync.Mutex{}},
		{"3336", 0, sync.Mutex{}},
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

	var selected *Server = &servers[0]

	for i := range servers {
		if selected.Connections > servers[i].Connections {
			selected = &servers[i]
		}
	}

	selected.Mutex.Lock()
	selected.Connections++
	selected.Mutex.Unlock()

	var target = "http://localhost:" + selected.Port + "/foo"

	resp, err := http.Get(target)

	if err != nil {
		log.Printf("Error: %v", err)
		http.Error(w, "Server unavailable", http.StatusInternalServerError)
		return
	}

	defer resp.Body.Close()
	io.Copy(w, resp.Body)

	selected.Mutex.Lock()
	selected.Connections--
	selected.Mutex.Unlock()
}

func handleResponse(w http.ResponseWriter, r *http.Request) {
	port := r.Host
	fmt.Fprintf(w, "Hello from %q on port %s\n", html.EscapeString(r.URL.Path), port)
	log.Default().Printf("Hello from %q on port %s\n", r.URL.Path, port)
}
