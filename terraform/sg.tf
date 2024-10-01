resource "aws_security_group" "sg-customer" {
    name = "SG-${var.projectName}-Customer"
    description = "Usado no EKS FastFood Customer"
    vpc_id = data.aws_vpc.vpc.id

    #Inbound
    ingress {
        description = "HTTP"
        from_port = 80
        to_port = 80
        protocol = "tcp"
        cidr_blocks = ["0.0.0.0/0"]
    }

    # Outbound
    egress {
        description = "All
        from_port = 0
        to_port = 0
        protocol = "-1"
        cidr_blocks = ["0.0.0.0/0"]
    }
}